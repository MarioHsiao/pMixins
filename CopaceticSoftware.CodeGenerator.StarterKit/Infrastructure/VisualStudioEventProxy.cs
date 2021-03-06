﻿//----------------------------------------------------------------------- 
// <copyright file="VisualStudioEventProxy.cs" company="Copacetic Software"> 
// Copyright (c) Copacetic Software.  
// <author>Philip Pittle</author> 
// <date>Wednesday, April 30, 2014 5:48:10 PM</date> 
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
using System.Collections.Generic;
using System.IO;
using CopaceticSoftware.CodeGenerator.StarterKit.Extensions;
using CopaceticSoftware.CodeGenerator.StarterKit.Infrastructure.IO;
using EnvDTE;
using EnvDTE80;
using VSLangProj;

namespace CopaceticSoftware.CodeGenerator.StarterKit.Infrastructure
{
    public interface IVisualStudioEventProxy : IDisposable
    {
        event EventHandler<ProjectAddedEventArgs> OnProjectAdded;
        event EventHandler<ProjectRemovedEventArgs> OnProjectRemoved;
        event EventHandler<ProjectReferenceAddedEventArgs> OnProjectReferenceAdded;
        event EventHandler<ProjectReferenceRemovedEventArgs> OnProjectReferenceRemoved;

        event EventHandler<ProjectItemAddedEventArgs> OnProjectItemAdded;
        event EventHandler<ProjectItemRemovedEventArgs> OnProjectItemRemoved;
        event EventHandler<ProjectItemRenamedEventArgs> OnProjectItemRenamed;
        event EventHandler<ProjectItemOpenedEventArgs> OnProjectItemOpened;
        event EventHandler<ProjectItemClosedEventArgs> OnProjectItemClosed;
        event EventHandler<ProjectItemSavedEventArgs> OnProjectItemSaved;
        event EventHandler<ProjectItemSavedEventArgs> OnProjectItemSaveComplete;

        event EventHandler<VisualStudioBuildEventArgs> OnBuildBegin;
        event EventHandler<VisualStudioBuildEventArgs> OnBuildDone;

        event EventHandler<EventArgs> OnSolutionClosing;
        event EventHandler<EventArgs> OnSolutionOpening;

        event EventHandler<CodeGeneratedEventArgs> OnCodeGenerated;

        void FireOnCodeGenerated(object sender, CodeGeneratorResponse response);
    }

    #region Event Arg Definitions

    #region Abstract Base Classes
    [Serializable]
    public abstract class VisualStudioEventArgs : EventArgs
    {
        public FilePath ProjectFullPath { get; set; }
        public abstract string GetDebugString();
    }

    [Serializable]
    public abstract class VisualStudioProjectEventArgs : VisualStudioEventArgs { }

    [Serializable]
    public abstract class VisualStudioProjectReferenceEventArgs : VisualStudioEventArgs
    {
        public FilePath ReferencePath { get; set; }
    }

    [Serializable]
    public abstract class VisualStudioClassEventArgs : VisualStudioEventArgs
    {
        public FilePath ClassFullPath { get; set; }

        public bool IsCSharpFile()
        {
            if (ClassFullPath.IsEmpty())
                return false;

            return ClassFullPath.Extension == "cs";
        }
    }
    #endregion

    #region Project Event Args
    [Serializable]
    public class ProjectAddedEventArgs : VisualStudioProjectEventArgs
    {
        public override string GetDebugString()
        {
            return string.Format(
                Strings.VisualStudioEventProjectAddedDebugString, ProjectFullPath);
        }
    }

    [Serializable]
    public class ProjectRemovedEventArgs : VisualStudioProjectEventArgs
    {
        public override string GetDebugString()
        {
            return string.Format(
                Strings.VisualStudioEventProjectRemovedDebugString, ProjectFullPath);
        }
    }

    [Serializable]
    public class ProjectReferenceAddedEventArgs : VisualStudioProjectReferenceEventArgs
    {
        public override string GetDebugString()
        {
            return string.Format(
                Strings.VisualStudioEventProjectReferenceAddedDebugString,
                ProjectFullPath,
                ReferencePath);
        }
    }

    [Serializable]
    public class ProjectReferenceRemovedEventArgs : VisualStudioProjectReferenceEventArgs
    {
        public override string GetDebugString()
        {
            return string.Format(
                Strings.VisualStudioEventProjectReferenceRemovedDebugString,
                ProjectFullPath,
                ReferencePath);
        }
    }


    #endregion

    #region Project Item Event Args
    [Serializable]
    public class ProjectItemAddedEventArgs : VisualStudioClassEventArgs
    {
        public override string GetDebugString()
        {
            return string.Format(
                Strings.VisualStudioEventClassAddedDebugString,
                ProjectFullPath,
                ClassFullPath);
        }
    }

    [Serializable]
    public class ProjectItemRemovedEventArgs : VisualStudioClassEventArgs
    {
        public override string GetDebugString()
        {
            return string.Format(
                Strings.VisualStudioEventClassRemovedDebugString,
                ProjectFullPath,
                ClassFullPath);
        }
    }

    [Serializable]
    public class ProjectItemRenamedEventArgs : VisualStudioClassEventArgs
    {
        public FilePath OldClassFileName { get; set; }

        public override string GetDebugString()
        {
            return string.Format(
                Strings.VisualStudioEventClassRenamedDebugString,
                ProjectFullPath,
                OldClassFileName,
                ClassFullPath);
        }

    }

    [Serializable]
    public class ProjectItemOpenedEventArgs : VisualStudioClassEventArgs
    {
        public IVisualStudioOpenDocumentReader DocumentReader { get; set; }

        public override string GetDebugString()
        {
            return string.Format(
                Strings.VisualStudioEventClassOpenedDebugString,
                ProjectFullPath,
                ClassFullPath);
        }
    }

    public class ProjectItemClosedEventArgs : VisualStudioClassEventArgs
    {
        public override string GetDebugString()
        {
            return string.Format(
                Strings.VisualStudioEventClassOpenedDebugString,
                ProjectFullPath,
                ClassFullPath);
        }
    }

    [Serializable]
    public class ProjectItemSavedEventArgs : VisualStudioClassEventArgs
    {
        public override string GetDebugString()
        {
            return string.Format(
                Strings.VisualStudioEventClassSavedDebugString,
                ProjectFullPath,
                ClassFullPath);
        }
    }
    #endregion

    #region Build Event Args

    public class VisualStudioBuildEventArgs : VisualStudioEventArgs
    {
        public vsBuildScope Scope { get; set; }
        public vsBuildAction BuildAction { get; set; }

        public override string GetDebugString()
        {
            return Strings.VisualStudioBuildEvent;
        }
    }
    #endregion

    #region Code Generated Event Args

    public class CodeGeneratedEventArgs : EventArgs
    {
        public CodeGeneratorResponse Response { get; set; }
    }
    #endregion

    #endregion

    public class VisualStudioEventProxy : IVisualStudioEventProxy
    {
        #region IVisualStudioEventProxy Event Declarations
        public event EventHandler<ProjectAddedEventArgs> OnProjectAdded;
        public event EventHandler<ProjectRemovedEventArgs> OnProjectRemoved;
        public event EventHandler<ProjectReferenceAddedEventArgs> OnProjectReferenceAdded;
        public event EventHandler<ProjectReferenceRemovedEventArgs> OnProjectReferenceRemoved;

        public event EventHandler<ProjectItemAddedEventArgs> OnProjectItemAdded;
        public event EventHandler<ProjectItemRemovedEventArgs> OnProjectItemRemoved;
        public event EventHandler<ProjectItemRenamedEventArgs> OnProjectItemRenamed;
        public event EventHandler<ProjectItemOpenedEventArgs> OnProjectItemOpened;
        public event EventHandler<ProjectItemClosedEventArgs> OnProjectItemClosed;
        public event EventHandler<ProjectItemSavedEventArgs> OnProjectItemSaved;
        public event EventHandler<ProjectItemSavedEventArgs> OnProjectItemSaveComplete;

        public event EventHandler<VisualStudioBuildEventArgs> OnBuildBegin;
        public event EventHandler<VisualStudioBuildEventArgs> OnBuildDone;

        public event EventHandler<EventArgs> OnSolutionClosing;
        public event EventHandler<EventArgs> OnSolutionOpening;
        public event EventHandler<CodeGeneratedEventArgs> OnCodeGenerated;
        #endregion

        /// <summary>
        /// Keep an instance of the dte objects to make sure it doesn't get GC'd
        /// http://stackoverflow.com/questions/5405167/dte2-events-dont-fire
        /// </summary>
        private DTE2 _dte;
        private SolutionEvents _solutionEvents;
        private DocumentEvents _documentEvents;
        private ProjectItemsEvents _projectItemsEvents;
        private BuildEvents _buildEvents;
        

        private readonly Dictionary<FilePath, ReferencesEvents> _projectSpecificReferenceEvents =
            new Dictionary<FilePath, ReferencesEvents>();
        
        public VisualStudioEventProxy(DTE2 dte)
        {
            _dte = dte;

            _buildEvents = _dte.Events.BuildEvents;
            _solutionEvents = _dte.Events.SolutionEvents;
            _documentEvents = _dte.Events.DocumentEvents;
            _projectItemsEvents = ((Events2)dte.Events).ProjectItemsEvents;

            AddDefaultEventHandlers();

            RegisterForSolutionLevelEvents();

            foreach (Project project in _dte.Solution.Projects)
            {
                RegisterForProjectLevelEvents(project.Object as VSProject);
            }
        }

        private void RegisterForSolutionLevelEvents()
        {
            _solutionEvents.ProjectAdded += project =>
                    {
                        RegisterForProjectLevelEvents(project.Object as VSProject);
                        OnProjectAdded(this, new ProjectAddedEventArgs
                        {
                            ProjectFullPath = new FilePath(project.FullName)
                        });
                    };

            _solutionEvents.ProjectRemoved += project =>
                {
                    if (string.IsNullOrEmpty(project.FullName))
                        return;

                    UnregisterForProjectLevelEvents(project as VSProject);
                    OnProjectRemoved(this, new ProjectRemovedEventArgs { ProjectFullPath = new FilePath(project.FullName) });
                };

            _solutionEvents.BeforeClosing += () => OnSolutionClosing(this, new EventArgs());

            _solutionEvents.Opened += () => OnSolutionOpening(this, new EventArgs());

           _projectItemsEvents.ItemAdded += item =>
                OnProjectItemAdded(this, new ProjectItemAddedEventArgs
                {
                    ProjectFullPath = new FilePath(item.ContainingProject.FullName),
                    ClassFullPath = new FilePath(item.GetFullName())
                });

            _projectItemsEvents.ItemRemoved += item =>
                OnProjectItemRemoved(this, new ProjectItemRemovedEventArgs
                {
                    ProjectFullPath = new FilePath(item.ContainingProject.FullName),
                    ClassFullPath = new FilePath(item.GetFullName())
                });

            _projectItemsEvents.ItemRenamed += (item, name) =>
                OnProjectItemRenamed(this, new ProjectItemRenamedEventArgs
                    {
                        ProjectFullPath = new FilePath(item.ContainingProject.FullName),
                        ClassFullPath = new FilePath(item.GetFullName()),
                        OldClassFileName = new FilePath(Path.GetDirectoryName(item.GetFullName()), name)
                    });

            _documentEvents.DocumentOpened += item =>
                OnProjectItemOpened(this, new ProjectItemOpenedEventArgs
                {
                    DocumentReader = new VisualStudioOpenDocumentReader(item),
                    ProjectFullPath = new FilePath(item.ProjectItem.ContainingProject.FullName),
                    ClassFullPath = new FilePath(item.FullName)
                });

            _documentEvents.DocumentClosing += item =>
                OnProjectItemClosed(this, new ProjectItemClosedEventArgs
                {
                    //ProjectFullPath = item.ProjectItem.ContainingProject.FullName,
                    ClassFullPath = new FilePath(item.FullName)
                });

            _documentEvents.DocumentSaved += item =>
            {
                var args = new ProjectItemSavedEventArgs
                {
                    ProjectFullPath = new FilePath(item.ProjectItem.ContainingProject.FullName),
                    ClassFullPath = new FilePath(item.FullName)
                };

                OnProjectItemSaved(this, args);

                OnProjectItemSaveComplete(this, args);
            };

            _buildEvents.OnBuildBegin += (scope, action) =>
                OnBuildBegin(this, new VisualStudioBuildEventArgs {Scope = scope, BuildAction = action});

            _buildEvents.OnBuildDone += (scope, action) =>
                 OnBuildDone(this, new VisualStudioBuildEventArgs { Scope = scope, BuildAction = action });
        }

        private void RegisterForProjectLevelEvents(VSProject project)
        {
            if (null == project)
                return;

            var projectFilePath = new FilePath(project.Project.FullName);

            if (_projectSpecificReferenceEvents.ContainsKey(projectFilePath))
                return;
            
            #region Reference Events
            var referenceEvents = project.Events.ReferencesEvents;

            referenceEvents.ReferenceAdded += reference =>
                OnProjectReferenceAdded(this, new ProjectReferenceAddedEventArgs
                {
                    ReferencePath = new FilePath(reference.Path),
                    ProjectFullPath = projectFilePath
                });
            
            referenceEvents.ReferenceRemoved += reference =>
                OnProjectReferenceRemoved(this, new ProjectReferenceRemovedEventArgs
                {
                    ReferencePath = new FilePath(reference.Path),
                    ProjectFullPath = projectFilePath
                });

            _projectSpecificReferenceEvents.Add(projectFilePath, referenceEvents);
            #endregion
        }

        private void UnregisterForProjectLevelEvents(VSProject project)
        {
            if (null == project || !_projectSpecificReferenceEvents.ContainsKey(new FilePath(project.Project.FullName)))
                return;

            _projectSpecificReferenceEvents.Remove(new FilePath(project.Project.FullName));
        }

        public void FireOnCodeGenerated(object sender, CodeGeneratorResponse response)
        {
            OnCodeGenerated(sender, new CodeGeneratedEventArgs{Response = response});
        }

        #region Misc Methods
        private void AddDefaultEventHandlers()
        {
            OnProjectAdded += (s, a) => { };
            OnProjectRemoved += (s, a) => { };
            OnProjectReferenceAdded += (s, a) => { };
            OnProjectReferenceRemoved += (s, a) => { };
            OnProjectItemAdded += (s, a) => { };
            OnProjectItemRemoved += (s, a) => { };
            OnProjectItemRenamed += (s, a) => { };
            OnProjectItemOpened += (s, a) => { };
            OnProjectItemClosed += (s, a) => { };
            OnProjectItemSaved += (s, a) => { };
            OnProjectItemSaveComplete += (s, a) => { };
            OnBuildBegin += (s, a) => { };
            OnBuildDone += (s, a) => { };
            OnSolutionClosing += (s, a) => { };
            OnSolutionOpening += (s, a) => { };
            OnCodeGenerated += (s, a) => { };
        }
        
        public void Dispose()
        {
            _dte = null;
            _solutionEvents = null;
            _documentEvents = null;
            _projectItemsEvents = null;

            OnProjectAdded.GetInvocationList().Map(del => OnProjectAdded -= (EventHandler<ProjectAddedEventArgs>)del);
            OnProjectRemoved.GetInvocationList().Map(del => OnProjectRemoved -= (EventHandler<ProjectRemovedEventArgs>)del);
            OnProjectReferenceAdded.GetInvocationList().Map(del => OnProjectReferenceAdded -= (EventHandler<ProjectReferenceAddedEventArgs>)del);
            OnProjectReferenceRemoved.GetInvocationList().Map(del => OnProjectReferenceRemoved -= (EventHandler<ProjectReferenceRemovedEventArgs>)del);
            OnProjectItemAdded.GetInvocationList().Map(del => OnProjectItemAdded -= (EventHandler<ProjectItemAddedEventArgs>)del);
            OnProjectItemRemoved.GetInvocationList().Map(del => OnProjectItemRemoved -= (EventHandler<ProjectItemRemovedEventArgs>)del);
            OnProjectItemRenamed.GetInvocationList().Map(del => OnProjectItemRenamed -= (EventHandler<ProjectItemRenamedEventArgs>)del);
            OnProjectItemOpened.GetInvocationList().Map(del => OnProjectItemOpened -= (EventHandler<ProjectItemOpenedEventArgs>)del);
            OnProjectItemClosed.GetInvocationList().Map(del => OnProjectItemClosed -= (EventHandler<ProjectItemClosedEventArgs>)del);
            OnProjectItemSaved.GetInvocationList().Map(del => OnProjectItemSaved -= (EventHandler<ProjectItemSavedEventArgs>)del);
            OnProjectItemSaveComplete.GetInvocationList().Map(del => OnProjectItemSaveComplete -= (EventHandler<ProjectItemSavedEventArgs>)del);
            OnBuildBegin.GetInvocationList().Map(del => OnBuildBegin -= (EventHandler<VisualStudioBuildEventArgs>)del);
            OnBuildDone.GetInvocationList().Map(del => OnBuildDone -= (EventHandler<VisualStudioBuildEventArgs>)del);
            OnSolutionClosing.GetInvocationList().Map(del => OnSolutionClosing -= (EventHandler<EventArgs>)del);
            OnSolutionOpening.GetInvocationList().Map(del => OnSolutionOpening -= (EventHandler<EventArgs>)del);
            OnCodeGenerated.GetInvocationList().Map(del => OnCodeGenerated -= (EventHandler<CodeGeneratedEventArgs>)del);
        }
        #endregion
    }
}
