# Solution Start Page - Dev Guide

This document gives generals advices and rules for developing in this solution.

## Solution Structure

The solution is structured into several projects, seperated into their concerns:
- SolutionStartPage --> The start-up project, hosted inside the Visual Studio start page container; only contains files required for the extension and the Visual Studio Gallery etc.
- SolutionStartPage.Core --> The general core functionality, for bootstrapping and the presenting logic
- SolutionStartPage.Shared --> Shared library used across the whole application (except SolutionStartPage project)
- SolutionStartPage.Proxies --> Contains proxie classes, used for runtime specific resolving of controls
- SolutionStartPage.Vs#### --> Visual Studio Version specific implementation of controls

The SolutionStartPage project (main project, containing the StartPage.xaml file) must reference
all the other projects (even if declared as "Unused Reference" by ReSharper), in order to include them inside the VSIX.