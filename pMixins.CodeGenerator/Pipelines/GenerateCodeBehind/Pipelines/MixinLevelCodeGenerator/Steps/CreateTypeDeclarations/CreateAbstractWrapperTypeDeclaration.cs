﻿//----------------------------------------------------------------------- 
// <copyright file="CreateAbstractWrapperTypeDeclaration.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Friday, July 25, 2014 4:47:53 PM</date> 
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

using System.Linq;
using CopaceticSoftware.CodeGenerator.StarterKit.Extensions;
using CopaceticSoftware.Common.Patterns;
using CopaceticSoftware.pMixins.CodeGenerator.Infrastructure;
using CopaceticSoftware.pMixins.CodeGenerator.Infrastructure.CodeGeneratorProxy;
using CopaceticSoftware.pMixins.CodeGenerator.Pipelines.GenerateCodeBehind.Pipelines.MixinLevelCodeGenerator.Steps.GenerateMembers;
using ICSharpCode.NRefactory.CSharp;

namespace CopaceticSoftware.pMixins.CodeGenerator.Pipelines.GenerateCodeBehind.Pipelines.MixinLevelCodeGenerator.Steps.CreateTypeDeclarations
{
    /// <summary>
    /// Creates the class definition for the Abstract Wrapper
    /// and assigns it to <see cref="MixinLevelCodeGeneratorPipelineState.AbstractMembersWrapper" />
    /// <code>
    /// <![CDATA[
    /// public class AbstractWrapper : Mixin{}
    /// ]]>
    /// </code>
    /// <remarks>
    /// Members are added later in <see cref="GenerateAbstractWrapperMembers"/>
    /// </remarks>
    /// </summary>
    public class CreateAbstractWrapperTypeDeclaration : IPipelineStep<MixinLevelCodeGeneratorPipelineState>
    {
        public bool PerformTask(MixinLevelCodeGeneratorPipelineState manager)
        {
            if (!manager.MixinGenerationPlan.AbstractWrapperPlan.GenrateAbstractWrapper)
                return true;

            var abstractWrapperTypeDeclaration = new TypeDeclaration
            {
                ClassType = ClassType.Class,
                Modifiers = 
                    (manager.MixinGenerationPlan.MixinAttribute.Mixin.GetDefinition().IsInternal
                    ? Modifiers.Internal
                    : Modifiers.Public),
                Name = manager.MixinGenerationPlan.AbstractWrapperPlan.AbstractWrapperClassName
            };

            //have abstract wrapper inherit from the protected wrapper
            abstractWrapperTypeDeclaration.BaseTypes.Add(
                new SimpleType(
                    (Identifier)
                        manager.ProtectedMembersWrapper.Descendants.OfType<Identifier>().First().Clone()));

            if (manager.MixinGenerationPlan.AbstractWrapperPlan.GenerateAbstractWrapperInExternalNamespace)
            {
                manager.TargetLevelCodeGeneratorPipelineState.CodeBehindSyntaxTree
                    .AddChildTypeDeclaration(
                        abstractWrapperTypeDeclaration,
                        manager.ExternalMixinSpecificAutoGeneratedNamespace);
            }
            else
            {
                new CodeGeneratorProxy(
                    manager.MixinAutoGeneratedContainerClass)
                    .AddNestedType(abstractWrapperTypeDeclaration);
            }

            //save abstract wrapper to the manager
            manager.AbstractMembersWrapper = abstractWrapperTypeDeclaration;

            return true;
        }
    }
}
