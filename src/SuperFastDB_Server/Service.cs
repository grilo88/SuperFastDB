using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SuperFastDB_Server
{
    public partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Quando implementado em uma classe derivada, é executado quando um comando Iniciar
        /// é enviado para o serviço pelo SCM (Gerenciador de Controle de Serviço) ou quando
        /// o sistema operacional é iniciado (para um serviço que inicia automaticamente).
        /// Especifica ações a serem tomadas quando o serviço for iniciado. 
        /// </summary>
        /// <param name="args">Dados passados pelo comando de início.</param>
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
        }

        /// <summary>
        /// Quando implementado em uma classe derivada, é executado quando o comando Parar
        /// é enviado para o serviço pelo SCM (Gerenciador de Controle de Serviço). Especifica
        /// ações a serem tomadas quando a execução do serviço parar.
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
        }

        /// <summary>
        /// Quando implementado em uma classe derivada, System.ServiceProcess.ServiceBase.OnContinue
        /// é executado quando o comando Continuar é enviado para o serviço pelo SCM (Gerenciador
        /// de Controle de Serviço). Especifica as ações a serem tomadas quando um serviço
        /// retoma o funcionamento normal após estar em pausa.
        /// </summary>
        protected override void OnContinue()
        {
            base.OnContinue();
        }

        /// <summary>
        /// Quando implementado em uma classe derivada, System.ServiceProcess.ServiceBase.OnCustomCommand(System.Int32)
        /// é executado quando o SCM (Gerenciador de Controle de Serviço) passa um comando
        /// personalizado para o serviço. Especifica as ações a serem aditadas quando ocorrer
        /// um comando com o valor do parâmetro especificado.
        /// </summary>
        /// <param name="command">A mensagem de comando enviada ao serviço.</param>
        protected override void OnCustomCommand(int command)
        {
            base.OnCustomCommand(command);
        }

        /// <summary>
        /// Quando implementado em uma classe derivada, é executado quando o comando Pausar
        /// é enviado para o serviço pelo SCM (Gerenciador de Controle de Serviço). Especifica
        /// ações a serem tomadas quando a execução for colocada em pausa.
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
        }

        /// <summary>
        /// Quando implementada em uma classe derivada, será executada quando o status de
        /// energia do computador for alterado. Isso se aplica a computadores laptop quando
        /// entram no modo suspenso, que não é o mesmo que um desligamento do sistema.
        /// </summary>
        /// <param name="powerStatus">Um System.ServiceProcess.PowerBroadcastStatus que indica uma 
        /// notificação do sistema sobre seu status de energia.</param>
        /// <returns>Quando implementada em uma classe derivada, as necessidades do seu aplicativo
        /// determinam qual valor retornar. Por exemplo, se um status de difusão QuerySuspend
        /// for passado, você poderá fazer com que seu aplicativo rejeite a consulta retornando
        /// false.</returns>
        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            switch (powerStatus)
            {
                case PowerBroadcastStatus.BatteryLow:
                    break;
                case PowerBroadcastStatus.OemEvent:
                    break;
                case PowerBroadcastStatus.PowerStatusChange:
                    break;
                case PowerBroadcastStatus.QuerySuspend:
                    break;
                case PowerBroadcastStatus.QuerySuspendFailed:
                    break;
                case PowerBroadcastStatus.ResumeAutomatic:
                    break;
                case PowerBroadcastStatus.ResumeCritical:
                    break;
                case PowerBroadcastStatus.ResumeSuspend:
                    break;
                case PowerBroadcastStatus.Suspend:
                    break;
                default:
                    break;
            }

            return base.OnPowerEvent(powerStatus);
        }

        /// <summary>
        /// Executa quando um evento de alteração é proveniente de uma sessão do servidor
        ///     Host da Sessão da Área de Trabalho Remota.
        /// </summary>
        /// <param name="changeDescription">Uma estrutura que identifica o tipo de alteração.</param>
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.ConsoleConnect:        // Uma sessão do console foi conectada.
                    break;
                case SessionChangeReason.ConsoleDisconnect:     // Uma sessão do console foi desconectada.
                    break;
                case SessionChangeReason.RemoteConnect:         // Uma sessão remota foi conectada.
                    break;
                case SessionChangeReason.RemoteDisconnect:      // Uma sessão remota foi desconectada.
                    break;
                case SessionChangeReason.SessionLogon:          // Um usuário fez logon em uma sessão.
                    break;
                case SessionChangeReason.SessionLogoff:         // Um usuário fez logoff em uma sessão.
                    break;
                case SessionChangeReason.SessionLock:           // A sessão foi bloqueada.
                    break;
                case SessionChangeReason.SessionUnlock:         // a sessão foi desbloqueada.
                    break;
                case SessionChangeReason.SessionRemoteControl:  // o status de controle remoto de uma sessão foi alterado.
                    break;
                default:
                    break;
            }

            base.OnSessionChange(changeDescription);
        }

        /// <summary>
        /// Quando implementado em uma classe derivada, é executado quando o sistema é desligado.
        /// Especifica o que deve ocorrer imediatamente antes do desligamento do sistema.
        /// </summary>
        protected override void OnShutdown()
        {
            base.OnShutdown();
        }
    }
}
