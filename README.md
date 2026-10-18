# CI/CD Lab for .NET

A tiny ASP.NET Core API wired to a real GitHub Actions pipeline, so you can break things on purpose and watch how CI/CD reacts.

```
push ─▶ 1 Build ─▶ 2 Test ─▶ 3 Package ─▶ 4 Staging ─▶ 5 Production
        dotnet      xUnit +    Docker      run image +   promote same
        build       coverage   image to    smoke tests   image (+ approval)
                    gate       GHCR
        └──────── CI ────────┘ └────────────── CD ───────────────┘
```

## What is in here

| Path | Purpose |
|---|---|
| `src/CicdLab.Api` | Minimal API: `GET /health` and `GET /api/discount?price=100&percent=10` |
| `tests/CicdLab.Tests` | xUnit unit tests and integration tests (`WebApplicationFactory`) |
| `Dockerfile` | Multi-stage image build |
| `.github/workflows/ci-cd.yml` | The pipeline |

Targets `net10.0`. If you only have the .NET 8 SDK, change `TargetFramework` in both `.csproj` files, the Docker image tags (`10.0` to `8.0`), the `Mvc.Testing` version (`10.*` to `8.*`) and `DOTNET_VERSION` in the workflow.

## Setup (about 5 minutes)

1. Create a **public** GitHub repo (free plans only support environment approvals on public repos) and push this folder to `main`.
2. Open the **Actions** tab and watch the first run. All green means your baseline works.
3. **Settings → Environments → production → Required reviewers**: add yourself. Now production waits for your approval (Continuous Delivery). Remove the reviewer to see Continuous Deployment.

Run locally any time:

```bash
dotnet test tests/CicdLab.Tests
dotnet run --project src/CicdLab.Api        # then open /health
docker build -t cicd-lab . && docker run -p 8080:8080 cicd-lab
```

## Experiments

Always work on a branch (`feature/...`), push, and watch which stage reacts. Merge to `main` to see staging and production.

| # | Change | Expected result | Concept |
|---|---|---|---|
| 1 | Open a pull request to `main` | Only Build and Test run. Package and deploy are skipped | CI on every PR |
| 2 | In `Program.cs` add `int unused = 5;` | **Build** fails (warnings are errors) | Fail fast, quality gates |
| 3 | In `DiscountCalculator.cs` change `/ 100m` to `/ 10m` | **Test** fails, nothing is packaged | Tests protect `main` |
| 4 | Add a new class with several `if` branches and no tests | **Test** fails the 70% coverage gate | Coverage gate |
| 5 | In the `Dockerfile` change `ASPNETCORE_HTTP_PORTS=8080` to `5000` | Build, Test and Package pass, **Staging** fails (unit tests can’t see this bug) | Why you need smoke tests |
| 6 | Change `COVERAGE_GATE` to `"0.99"` in the workflow | Test fails with a clear message | Pipeline is code |
| 7 | Merge a good change with reviewers on `production` | Pipeline pauses at **Production** until you click Approve | Continuous Delivery |
| 8 | Remove the reviewer and merge again | Goes straight to production | Continuous Deployment |
| 9 | Make `/health` return `Results.StatusCode(500)` | **Test** fails on the health test, so the bad change never reaches Package | Defence in layers |

## Stretch goals

- Rollback: re-run the `deploy-production` job from an older successful run.
- Add `dotnet format --verify-no-changes` as a lint step.
- Add an Azure deploy step (`azure/webapps-deploy`) to Staging using OIDC login.
- Add a CodeQL or `dotnet list package --vulnerable` security scan stage.
- Port the same pipeline to Azure DevOps (`azure-pipelines.yml`) and compare.

## Notes

- Build and Test each restore and compile on their own runner to keep jobs simple. The **image** is built once in Package and the exact same image is promoted through Staging and Production.
- Images go to GitHub Container Registry (`ghcr.io`) using the built-in `GITHUB_TOKEN`, so no secrets are needed.
