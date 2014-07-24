﻿//----------------------------------------------------------------------- 
// <copyright file="TargetLevelCodeGeneratorPipeline.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Wednesday, July 23, 2014 11:34:24 AM</date> 
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

using CopaceticSoftware.pMixins.Attributes;
using CopaceticSoftware.pMixins.CodeGenerator.Infrastructure;
using CopaceticSoftware.pMixins.CodeGenerator.Infrastructure.CodeGenerationPlan;
using CopaceticSoftware.pMixins.CodeGenerator.Pipelines.CreateCodeGenerationPlan;
using CopaceticSoftware.pMixins.CodeGenerator.Pipelines.GenerateCodeBehind.Pipelines.MixinLevelCodeGenerator.Steps.CreateTypeDeclarations;
using CopaceticSoftware.pMixins.CodeGenerator.Pipelines.GenerateCodeBehind.Pipelines.TargetLevelCodeGenerator.Steps.CreateTypeDeclarations;
using CopaceticSoftware.pMixins.CodeGenerator.Pipelines.GenerateCodeBehind.Steps;
using ICSharpCode.NRefactory.CSharp;

namespace CopaceticSoftware.pMixins.CodeGenerator.Pipelines.GenerateCodeBehind.Pipelines.TargetLevelCodeGenerator
{
    public class TargetLevelCodeGeneratorPipelineState : IGenerateCodePipelineState
    {
        public TargetLevelCodeGeneratorPipelineState(IGenerateCodePipelineState baseState)
        {
            CommonState = baseState.CommonState;
            CreateCodeGenerationPlanPipeline = baseState.CreateCodeGenerationPlanPipeline;
            CodeBehindSyntaxTree = baseState.CodeBehindSyntaxTree;
        }

        /// <summary>
        /// The <see cref="CodeGenerationPlan"/> for the current
        /// Target.
        /// </summary>
        public CodeGenerationPlan CodeGenerationPlan { get; set; }

        #region IGenerateCodePipelineState

        public IPipelineCommonState CommonState { get; private set; }

        /// <summary>
        /// The State from the previous Resolve Members
        /// step.
        /// </summary>
        public ICreateCodeGenerationPlanPipelineState CreateCodeGenerationPlanPipeline { get; private set; }

        /// <summary>
        /// Dictionary of <see cref="MemberWrapper"/> representing
        /// each <see cref="IPipelineCommonState.SourcePartialClassDefinitions"/>'s
        /// <see cref="IPMixinAttribute"/>'s Members.
        /// </summary>
        //Dictionary<TypeDeclaration, IList<MemberWrapper>> MixinMembers { get; }

        public SyntaxTree CodeBehindSyntaxTree { get; private set; }

        #endregion

        #region Type Declarations
        /// <summary>
        /// Reference to the original Target source code.
        /// Set in <see cref="RunTargetLevelCodeGeneratorForEachTarget"/>.
        /// </summary>
        public TypeDeclaration TargetSourceTypeDeclaration { get; set; }

        /// <summary>
        /// Type decalration for the Target's Code Behind.
        /// Set in <see cref="CreateTargetCodeBehindTypeDeclaration"/>.
        /// </summary>
        public TypeDeclaration TargetCodeBehindTypeDeclaration { get; set; }

        /// <summary>
        /// The global container for generated wrapper classes.
        /// Defined in <see cref="GlobalAutoGeneratedContainerClass"/>.
        /// 
        /// <see cref="CreateMixinLevelAutoGeneratedContainerClass"/>
        /// adds a Mixin specific container.
        /// </summary>
        public TypeDeclaration GlobalAutoGeneratedContainerClass { get; set; }
        #endregion
    }
}
