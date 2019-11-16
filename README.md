# Azure DevOps Workshop

## Prerequisites

### Git & GitHub Account
You will need a GitHub account as well as basic knowledge of Git for forking, committing, and pushing code within a repository. If you do not already have a GitHub account, you can [sign up at GitHub.com](https://github.com)


### Azure DevOps Account
To learn about Azure DevOps you will need an Azure DevOps account. If you do not already have an Azure DevOps account, you can [sign up at azure.microsoft.com/services/devops](https://azure.microsoft.com/services/devops/); the free tier is sufficient for this workshop.


### Azure Subscription
In addition to an Azure DevOps account, an Azure subscription is also recommended. An Azure subscription will be used to host deployed websites and working with release management, continuous delivery, and continuous deployment. If needed, [sign up at azure.microsoft.com/services/app-service](https://azure.microsoft.com/services/app-service/web/). _Note:_ the signup process will ask you for a phone number and credit card for identity verification, but you will not be charged unless you change your subscription tier.


### Two Azure App Service Applications
Within your Azure Subscription, create two free-tier Azure App Service Application instances to simulate Test and Production environments.

Ours are called `workshop-dev` and `workshop-prod`.


## Preface

### Step 1: Maintain a source control repository

 1. Fork the Sample Code: https://github.com/aranasoft/learn-azureDevOps

This workshop is not about building a new website in some various technology, but instead on automation for building, testing, and deploying and application using Azure DevOps. This code project, little more than a default template for an AspNetCore web project, will provide a baseline application for the workshop. Forking this repository allow you to make modifications to the codebase and commits to the repository to learn the fundamentals and workflow of continuous integration, delivery, and deployment.


## Integration

### Step 2: Automate the build

 1. Create a new pipeline
 2. Repository: GitHub with a Personal Access Token
 3. Pipeline: Rename. Set Agent pool to `Azure Pipelines` and a specification of `ubuntu-16.04`
 4. Agent Job: Rename
 5. Create a variable `BuildConfiguration` set to `Release`
 5. Add a `.NET Core` Task. Command: Restore.
 6. Add a `.NET Core` Task. Command: Build. Args: `--no-restore --configuration $(BuildConfiguration)`
 7. Save and Queue


### Step 3: Make the build self-testing

 1. Edit pipeline
 2. Add a `.NET Core` Task. Command: Test. Path: `*.tests/*.tests.csproj` Arg: `--no-restore --no-build --configuration $(BuildConfiguration)`
 3. Save and Queue


### Step 4: Everyone commits to the repository every day

A purely human element, this is a task that cannot be governed with automation.


### Step 5: Every commit to the repository should be built on an integration machine

 1. Edit pipeline; Go to the Triggers tab
 2. Enable continuous integration. Do _not_ batch changes
 3. Enable for all branches
 4. Enable Pull Request validation
 5. Enable a scheduled build for 1:00am, every day.
 6. Create a `PowerShell Script` task, inline script. Code: http://bit.ly/branchQuality
 7. Save and Queue


### Step 6: Everyone can see the result of the latest build

 1. Go to Project Settings; Notifications tab
 2. Add a new Subscription for Changed By when Build Fails
 3. Edit pipeline; Go to the Options tab
 4. Add the Badge to your README.md; commit
 5. Save and Queue


### Step 7: Keep the build fast

 1. Edit pipeline; Go to the Tasks tab
 2. Set the Timeout for the Build task to 5 minutes.
 3. Set the Timeout for the Job to 10 minutes.
 4. Save and Queue


## Delivery

### Step 8: Make it easy to get the latest deliverables

 1. Add a `.NET Core` Task. Command: Publish. Zip with Path. Publish Web Projects. Arg: ` --no-restore --no-build --configuration $(BuildConfiguration) --output $(Build.ArtifactStagingDirectory)`
 2. Add a `Publish Build Artifacts` Task.
 3. Save and Queue


### Step 9: Test in a clone of the production environment

 1. Create a new Release Pipeline
 2. Create a new Release Artifact from your Build Pipeline
 3. Enable the Artifact's Continuous Deployment Trigger
 4. Create a `Test` Stage. Enable the `After Release` Trigger
 5. Create an `Azure App Service Deploy` task from your `learn-azureDevOps.zip` package to your subscription's `workshop-dev` instance
 6. Save; trigger a build


### Step 10: Automate the production deployment

 1. Edit the Release pipeline
 2. Clone the Test stage; rename cloned stage to Production
 3. Enable Production pre-deployment After Stage trigger
 4. Enable Production pre-deployment approvals
 5. Modify Production job deploy task to use `workshop-prod`
 6. Save; trigger a build
:ne

## Code Managed

With an understanding of Azure DevOps using the classic interface,
rebuilding the pipelines in YAML allows the source repository to manage
its own build configuration.

### Step 11: Pipelines

 1. Create an empty-pipeline file
 2. Connect the file to Azure DevOps

### Step 12: Pipeline Steps

 1. Add `DotNetCoreCLI@2` tasks for Restore, Build, & Test
 2. Add `BuildConfiguration` queue variable
 3. Save

### Step 13: Pipeline Jobs

 1. Edit pipeline
 2. Wrap existing steps in a job
 3. Create Publishing steps within existing job
 4. Save

### Step 14: Pipeline Triggers

 1. Edit pipeline
 2. Add a branch trigger to the pipeline
 3. Add a pr trigger to the pipeline
 4. Add a cron trigger to the pipeline
 5. Save

### Step 15: Pipeline Deployments

 1. Enable Multi-Stage Feature
 2. Add a Service Connection to your Subscription
 3. Wrap all existing jobs in a Build Stage
 4. Add a new Deployment Stage, with a Deployment Job to Test
 5. Save

### Step 16: Approvals and Conditions

 1. Add a Production environment
 2. Configure Production for Pre-Stage Approvals
 3. Edit Pipeline
 4. Add Stage for Production Deployment
 5. Add branch condition to Production Deployment stage: `and(succeeded(), eq(variables['build.sourceBranch'], 'refs/heads/master'))`
 4. Save


