﻿//----------------------------------------------------------------------- 
// <copyright file="pMixinVisualStudioCodeGenerator.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Saturday, March 8, 2014 10:44:15 PM</date> 
// Licensed under the Apache License, Version 2.0,
// you may not use this file except in compliance with this License.
//  
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an 'AS IS' BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright> 
//-----------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
using System.Text;
using CopaceticSoftware.CodeGenerator.StarterKit;
using CopaceticSoftware.CodeGenerator.StarterKit.Infrastructure;
using CopaceticSoftware.CodeGenerator.StarterKit.Infrastructure.VisualStudioSolution;
using CopaceticSoftware.Common.Extensions;
using CopaceticSoftware.pMixins.CodeGenerator;
using CopaceticSoftware.pMixins_VSPackage.Infrastructure;
using EnvDTE80;
using Microsoft.Samples.VisualStudio.GeneratorSample;
using Microsoft.VisualStudio.Shell;
using VSLangProj80;

namespace CopaceticSoftware.pMixins_VSPackage
{
    [ComVisible(true)]
    [Guid("3E3CAED9-8C24-4332-A774-059F50FF38D6")]
    [ProvideObject(typeof(pMixinsVisualStudioCodeGenerator))]
    [CodeGeneratorRegistration(
        typeof(pMixinsVisualStudioCodeGenerator),
        "C# pMixin Code Generator", 
        vsContextGuids.vsContextGuidVCSProject, 
        GeneratesDesignTimeSource = true)]
    public class pMixinsVisualStudioCodeGenerator : BaseCodeGeneratorWithSite
    {
        private readonly VisualStudioEventProxyFactory _eventProxyFactory = new VisualStudioEventProxyFactory();

        public pMixinsVisualStudioCodeGenerator()
        {
            log4net.Config.BasicConfigurator.Configure();
        }

        protected override string GetDefaultExtension()
        {
            return ".mixin.cs";
        }

        protected override byte[] GenerateCode(string inputFileContent)
        {
            try
            {
                var codeGeneratorResponse = GetCodeGeneratorResponse(inputFileContent);

                #region Write Messages to Output Pane
                var outputPane = GetOutputPane(GetVSProject().DTE);

                foreach (var logMessage in codeGeneratorResponse.LogMessages)
                    outputPane.OutputString(logMessage.EnsureEndsWith(Environment.NewLine));
                #endregion

                #region Write Errors / Warnings
                foreach (var error in codeGeneratorResponse.Errors)
                    switch (error.Severity)
                    {
                        case CodeGenerationError.SeverityOptions.Error:
                            GeneratorError(4, error.Message, error.Line, error.Column);
                            break;

                        case CodeGenerationError.SeverityOptions.Warning:
                            GeneratorWarning(1, error.Message, error.Line, error.Column);
                            break;
                    }
                #endregion

                return Encoding.UTF8.GetBytes(codeGeneratorResponse.GeneratedCodeSyntaxTree.GetText());
            }
            catch (Exception e)
            {
                return Encoding.UTF8.GetBytes(
                    string.Format("/*Unhandled exception: {0} {1}  */",
                        e.Message,
                        e.StackTrace));
            }
        }

        private CodeGeneratorResponse GetCodeGeneratorResponse(string inputFileContent)
        {
            var solutionManager = new SolutionManager(
                    GetVSProject().DTE.Solution.FullName,
                    _eventProxyFactory.BuildVisualStudioEventProxy(() => (DTE2)GetVSProject().DTE));

            var codeGeneratorContext =
                new CodeGeneratorContextFactory(solutionManager)
                    .GenerateContext(inputFileContent, GetProjectItem().Name, GetProject().FullName);

            return
                new pMixinPartialCodeGenerator()
                    .GeneratePartialCode(codeGeneratorContext);

        }
    }
}