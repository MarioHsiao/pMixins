﻿//----------------------------------------------------------------------- 
// <copyright file="ServiceLocator.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Saturday, May 3, 2014 4:11:20 PM</date> 
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

using CopaceticSoftware.CodeGenerator.StarterKit.Infrastructure;
using CopaceticSoftware.CodeGenerator.StarterKit.Ninject;
using CopaceticSoftware.pMixins.VisualStudio.Ninject;
using CopaceticSoftware.pMixins_VSPackage.CodeGenerators;
using EnvDTE80;
using Ninject;

namespace CopaceticSoftware.pMixins_VSPackage.Infrastructure
{
    public static class ServiceLocator
    {
        public static IKernel Kernel { get; private set; }

        public static void Initialize(IVisualStudioWriter visualStudioWriter, IVisualStudioEventProxy visualStudioEventProxy, IVisualStudioProjectHelper visualStudioProjectHelper)
        {
            Kernel = new StandardKernel(
                new StandardModule(),
                new pMixinsStandardModule());

            Kernel.Bind<IVisualStudioWriter>().ToMethod(c => visualStudioWriter).InSingletonScope();

            Kernel.Bind<IVisualStudioEventProxy>().ToMethod(c => visualStudioEventProxy).InSingletonScope();

            Kernel.Bind<IVisualStudioProjectHelper>().ToMethod(c => visualStudioProjectHelper).InSingletonScope();
        }
    }
}
