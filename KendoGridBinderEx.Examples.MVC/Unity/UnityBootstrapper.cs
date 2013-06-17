﻿using System.Data.Entity;
using System.Web.Mvc;
using EntityFramework.Patterns;
using KendoGridBinderEx.Examples.MVC.Data.Repository;
using KendoGridBinderEx.Examples.MVC.Data.Service;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Unity.Mvc4;

namespace KendoGridBinderEx.Examples.MVC.Unity
{
    public static class UnityBootstrapper
    {
        private static readonly IUnityContainer UnityContainer = new UnityContainer();

        public static IUnityContainer Container { get { return UnityContainer; } }

        public static void Initialise()
        {
            UnityContainer.LoadConfiguration();

            // Registering interfaces of Unit Of Work & Generic Repository
            UnityContainer.RegisterType(typeof(IRepositoryEx<>), typeof(RepositoryEx<>));
            UnityContainer.RegisterType(typeof(IUnitOfWork), typeof(UnitOfWork));

            UnityContainer.RegisterType<IObjectSetFactory>(new InjectionFactory(con => con.Resolve<DbContextAdapter>()));

            UnityContainer.RegisterType<IObjectContext>(new InjectionFactory(con => con.Resolve<DbContextAdapter>()));

            UnityContainer.RegisterInstance(new DbContextAdapter(UnityContainer.Resolve<DbContext>()), new PerThreadLifetimeManager());

            UnityContainer.RegisterType<IEmployeeService>(new InjectionFactory(con => con.Resolve<EmployeeService>()));
            UnityContainer.RegisterType<IProductService>(new InjectionFactory(con => con.Resolve<ProductService>()));
            UnityContainer.RegisterType<ICompanyService>(new InjectionFactory(con => con.Resolve<CompanyService>()));

            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityContainer));
        }
    }
}