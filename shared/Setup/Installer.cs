﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.Reflection;
using System.ServiceProcess;
using System.Text;

namespace SuperFastDB
{
    public abstract class InstallableServiceBase : ServiceBase
    {
        /// <summary> Retorna o tipo do serviço de chamada(subclasse de InstallableServiceBase)
        /// <returns></returns>
        protected static Type getMyType()
        {
            Type t = typeof(InstallableServiceBase);
            MethodBase ret = MethodBase.GetCurrentMethod();
            Type retType = null;
            try
            {
                StackFrame[] frames = new StackTrace().GetFrames();
                foreach (StackFrame x in frames)
                {
                    ret = x.GetMethod();

                    Type t1 = ret.DeclaringType;

                    if (t1 != null && !t1.Equals(t) && !t1.IsSubclassOf(t))
                    {


                        break;
                    }
                    retType = t1;
                }
            }
            catch
            {

            }
            return retType;
        }
        /// <summary> retorna AssemblyInstaller para o serviço de chamada (subclasse de InstallableServiceBase) </summary>
        /// <returns></returns>
        protected static AssemblyInstaller GetInstaller()
        {
            Type t = getMyType();
            AssemblyInstaller installer = new AssemblyInstaller(
                t.Assembly, null);
            installer.UseNewContext = true;
            return installer;
        }

        private bool IsInstalled()
        {
            using (ServiceController controller =
                new ServiceController(this.ServiceName))
            {
                try
                {
                    ServiceControllerStatus status = controller.Status;
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }

        private bool IsRunning()
        {
            using (ServiceController controller =
                new ServiceController(this.ServiceName))
            {
                if (!this.IsInstalled()) return false;
                return (controller.Status == ServiceControllerStatus.Running);
            }
        }
        /// <summary>
        /// Método protegido para ser chamado por um método público dentro do serviço real.
        /// ie: no serviço real
        ///    new internal  void InstallService()
        ///    {
        ///        base.InstallService();
        ///    }
        /// </summary>
        protected void InstallService()
        {
            if (this.IsInstalled()) return;

            try
            {
                using (AssemblyInstaller installer = GetInstaller())
                {

                    IDictionary state = new Hashtable();
                    try
                    {
                        installer.Install(state);
                        installer.Commit(state);
                    }
                    catch
                    {
                        try
                        {
                            installer.Rollback(state);
                        }
                        catch { }
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Método protegido para ser chamado por um método público dentro do serviço real
        /// ie: no serviço real
        ///    new internal  void UninstallService()
        ///    {
        ///        base.UninstallService();
        ///    }
        /// </summary>
        protected void UninstallService()
        {
            if (!this.IsInstalled()) return;

            if (this.IsRunning())
            {
                this.StopService();
            }
            try
            {
                using (AssemblyInstaller installer = GetInstaller())
                {
                    IDictionary state = new Hashtable();
                    try
                    {
                        installer.Uninstall(state);
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void StartService()
        {
            if (!this.IsInstalled()) return;

            using (ServiceController controller =
                new ServiceController(this.ServiceName))
            {
                try
                {
                    if (controller.Status != ServiceControllerStatus.Running)
                    {
                        controller.Start();
                        controller.WaitForStatus(ServiceControllerStatus.Running,
                            TimeSpan.FromSeconds(10));
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        private void StopService()
        {
            if (!this.IsInstalled()) return;
            using (ServiceController controller =
                new ServiceController(this.ServiceName))
            {
                try
                {
                    if (controller.Status != ServiceControllerStatus.Stopped)
                    {
                        controller.Stop();
                        controller.WaitForStatus(ServiceControllerStatus.Stopped,
                             TimeSpan.FromSeconds(10));
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
