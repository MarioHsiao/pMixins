﻿//----------------------------------------------------------------------- 
// <copyright file="MixinDoesNotHaveParameterlessConstructor.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Wednesday, January 29, 2014 10:57:24 PM</date> 
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

using CopaceticSoftware.pMixins.CodeGenerator.Tests.Extensions;
using NBehave.Spec.NUnit;
using NUnit.Framework;

namespace CopaceticSoftware.pMixins.CodeGenerator.Tests.IntegrationTests.CompileTests.AdvancedMixinTypes
{
    [TestFixture]
    public class MixinDoesNotHaveParameterlessConstructor : GenerateCodeAndCompileTestBase
    {
        protected override string SourceCode
        {
            get
            {
                return
                    @"
                using pMixin.AutoGenerated.Test.Target.Test.MixinWithoutParameterlessConstructor;
                using pMixin.AutoGenerated.Test.Target.Test.MixinWithoutParameterlessConstructorThatIsProtected;

                namespace Test
                {
                    public class MixinWithoutParameterlessConstructor
                    {
                        private string _name;

                        public MixinWithoutParameterlessConstructor(string name)
                        {
                            _name = name;
                        }

                        public string GetName(){return _name;}                                
                    }

                    public class MixinWithoutParameterlessConstructorThatIsProtected
                    {
                        private string _name;

                        protected MixinWithoutParameterlessConstructorThatIsProtected(string name)
                        {
                            _name = name;
                        }

                        public string GetNameProtected(){return _name;}                                
                    }

                    [CopaceticSoftware.pMixins.Attributes.pMixin(
                        Mixin = typeof (Test.MixinWithoutParameterlessConstructor),                        
                        ExplicitlyInitializeMixin=true)]
                    [CopaceticSoftware.pMixins.Attributes.pMixin(
                        Mixin = typeof (Test.MixinWithoutParameterlessConstructorThatIsProtected),                        
                        ExplicitlyInitializeMixin=true)]
                    public partial class Target
                    {
                        MixinWithoutParameterlessConstructor 
                        CopaceticSoftware.pMixins.Infrastructure.IMixinConstructorRequirement<Test.MixinWithoutParameterlessConstructor>
                        .InitializeMixin()
                        {
                            return new MixinWithoutParameterlessConstructor(""Initialized in Target"");
                        }

                        MixinWithoutParameterlessConstructorThatIsProtected 
                        CopaceticSoftware.pMixins.Infrastructure.IMixinConstructorRequirement<Test.MixinWithoutParameterlessConstructorThatIsProtected>
                        .InitializeMixin()
                        {
                            return new __pMixinAutoGenerated.Test_MixinWithoutParameterlessConstructorThatIsProtected.MixinWithoutParameterlessConstructorThatIsProtectedAbstractWrapper
                                (this, ""Initialized in Target-Protected"");
                        }
                    }                        
                }
                ";
            }
        }

        [Test]
        public void CanCallMixinMethods()
        {
            CompilerResults
                .ExecuteMethod<string>(
                    "Test.Target",
                    "GetName")
                .ShouldEqual("Initialized in Target");

            CompilerResults
                .ExecuteMethod<string>(
                    "Test.Target",
                    "GetNameProtected")
                .ShouldEqual("Initialized in Target-Protected"); 
        }

    }
}

              
