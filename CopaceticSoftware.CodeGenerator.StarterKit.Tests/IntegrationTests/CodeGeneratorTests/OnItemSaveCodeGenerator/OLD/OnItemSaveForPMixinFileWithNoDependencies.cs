﻿//----------------------------------------------------------------------- 
// <copyright file="OnItemSaveForPMixinFileWithNoDependencies.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Sunday, May 11, 2014 8:21:15 PM</date> 
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
using System.Linq;
using CopaceticSoftware.CodeGenerator.StarterKit.Infrastructure;
using CopaceticSoftware.pMixins.Tests.Common.Extensions;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using Rhino.Mocks;

namespace CopaceticSoftware.CodeGenerator.StarterKit.Tests.IntegrationTests.CodeGeneratorTests.OnItemSaveCodeGenerator.OLD
{
    public class OnItemSaveForPMixinFileWithNoDependencies : OnItemSaveCodeGeneratorTestBase
    {
        protected readonly MockSourceFile _sourceFile =
            new MockSourceFile
            {
                Source = @"
                    namespace Testing{
                        public class Mixin{ public int AMethod(){return 1;} }

                        [CopaceticSoftware.pMixins.Attributes.pMixin(Mixin = typeof(Mixin))]                                        
                        public partial class Target  {}
                    }
                    "
            };

        public override void MainSetup()
        {
            base.MainSetup();

            _MockSolution.Projects.Add(
                new MockProject
                {
                    MockSourceFiles =
                    {
                        _sourceFile
                    }
                });

            //Simulate Solution Opening event
            EventProxy.FireOnSolutionOpening(this, new EventArgs());

            //Simulate Item Saved
            EventProxy.FireOnProjectItemSaved(this,
                new ProjectItemSavedEventArgs
                {
                    ClassFullPath = _sourceFile.FileName,
                    ProjectFullPath = _MockSolution.Projects[0].FileName
                });
        }

        [Test]
        public void CodeBehindFileIsGeneratedAndCompiles()
        {
            Assert.True(
                _MockSolution.AllMockFiles().Any(x => x.FileName.EndsWith("mixin.cs")),
                "Found a mixin.cs code behind file.");

            _MockCodeBehindFileHelper.AssertWasCalled(
                x => x.GetOrAddCodeBehindFile(Arg<string>.Is.Equal(_sourceFile.FileName)),
                options => options.Repeat.AtLeastOnce());

            var mixinFileName = _sourceFile.FileName.Replace(".cs", ".mixin.cs");

            _MockFileWrapper.AssertWasCalled(
                x => x.WriteAllText(Arg<string>.Is.Equal(mixinFileName), Arg<string>.Is.Anything),
                options => options.Repeat.AtLeastOnce());

            var compilerResults =
              AssertProjectCompiles(_MockSolution.Projects[0]);

            compilerResults
               .ExecuteMethod<int>(
                   "Testing.Target",
                   "AMethod")
               .ShouldEqual(1); 
        }
    }
}
