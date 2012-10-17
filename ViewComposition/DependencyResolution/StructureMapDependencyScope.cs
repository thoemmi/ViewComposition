// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMapDependencyScope.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Microsoft.Practices.ServiceLocation;
using StructureMap;

namespace ViewComposition.DependencyResolution {
    public class StructureMapDependencyScope : ServiceLocatorImplBase, IDependencyScope {
        protected readonly IContainer Container;

        public StructureMapDependencyScope(IContainer container) {
            if (container == null) {
                throw new ArgumentNullException("container");
            }

            Container = container;
        }

        public void Dispose() {
            Container.Dispose();
        }

        public override object GetService(Type serviceType) {
            if (serviceType == null) {
                return null;
            }

            try {
                return serviceType.IsAbstract || serviceType.IsInterface
                           ? Container.TryGetInstance(serviceType)
                           : Container.GetInstance(serviceType);
            } catch {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            return Container.GetAllInstances(serviceType).Cast<object>();
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType) {
            return Container.GetAllInstances(serviceType).Cast<object>();
        }

        protected override object DoGetInstance(Type serviceType, string key) {
            if (string.IsNullOrEmpty(key)) {
                return serviceType.IsAbstract || serviceType.IsInterface
                           ? Container.TryGetInstance(serviceType)
                           : Container.GetInstance(serviceType);
            }

            return Container.GetInstance(serviceType, key);
        }
    }
}