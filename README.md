# What is this

It is an moduling framework using `Microsoft.Extensions.DependencyInjection`

# How to use

1. Install package from nuget

Install `Structing.Core` and `Structing`

2. Write a module

Implement interface `IModuleEntry` or extends class `AutoModuleEntity`

3. Register/Ready/Start

Calling there methods to active modules

4. Close

Call `CloseAsync` to close modules

# Why need to do that

In many cases, you need to architecture your application. How should it be developed by structure, and modular design is always a good choice.

Using **MSDI** to manage services is a fast decoupling method.

# TargetPlatforms

- Structing.Core

Can run at `netstandard1.0` or `netstandard2.0`

- Structing

Can run at `net45` or `netstandard2.0`

# Features

- [ ] Add startup pipelines
- [ ] Add samples
- [ ] Add more tests