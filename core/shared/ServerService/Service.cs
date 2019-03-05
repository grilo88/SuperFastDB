using SuperFastDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using SuperFastDB.Server;

namespace SuperFastDB.ServerService
{
    public partial class Service : ServiceBase
    {
        Instance inst_server; // Instância do Servidor

        public Service()
        {
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
            //Primeira execução?
            if (inst_server == null)
            {
                inst_server = new Instance(); // Instancia o Serviço do Servidor
            }

            inst_server.StartServer(); // Inicializa o Servidor

            base.OnStart(args); // Executa o método base
        }

        /// <summary>
        /// Quando implementado em uma classe derivada, é executado quando o comando Parar
        /// é enviado para o serviço pelo SCM (Gerenciador de Controle de Serviço). Especifica
        /// ações a serem tomadas quando a execução do serviço parar.
        /// </summary>
        protected override void OnStop()
        {
            inst_server.StopServer();

            base.OnStop(); // Executa o método base
        }

        /// <summary>
        /// Quando implementado em uma classe derivada, System.ServiceProcess.ServiceBase.OnContinue
        /// é executado quando o comando Continuar é enviado para o serviço pelo SCM (Gerenciador
        /// de Controle de Serviço). Especifica as ações a serem tomadas quando um serviço
        /// retoma o funcionamento normal após estar em pausa.
        /// </summary>
        protected override void OnContinue()
        {
            inst_server.StartServer();

            base.OnContinue(); // Executa o método base
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
            inst_server.StopServer();

            base.OnPause(); // Executa o método base
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
                case PowerBroadcastStatus.BatteryLow:           // A bateria está fraca
                    // TODO: Implementar case "A bateria está fraca"
                    break;
                case PowerBroadcastStatus.OemEvent:             // Evento de OEM
                    // TODO: Implementar case "Evento OEM"
                    break;
                case PowerBroadcastStatus.PowerStatusChange:    // Mudança de status da bateria
                    // TODO: Implementar case "Mudança de status da bateria"
                    break;
                case PowerBroadcastStatus.QuerySuspend:         // Permissão para suspender o computador
                    // TODO: Implementar case "Permissão para suspender o computador"
                    break;
                case PowerBroadcastStatus.QuerySuspendFailed:   // O sistema não obteve permissão para suspender o computador
                    // TODO: Implementar case "O sistema não obteve permissão para suspender o computador"
                    break;
                case PowerBroadcastStatus.ResumeAutomatic:      // O computador foi ativado automaticamente para lidar com um evento
                    // TODO: Implementar case "O computador foi ativado automaticamente para lidar com um evento"
                    break;
                case PowerBroadcastStatus.ResumeCritical:       // O sistema retomou a operação após uma suspensão crítica causada por uma bateria com falha
                    // TODO: Implementar case "O sistema retomou a operação após uma suspensão crítica causada por uma bateria com falha"
                    break;
                case PowerBroadcastStatus.ResumeSuspend:        // O sistema retomou a operação após ter sido suspenso
                    // TODO: Implementar case "O sistema retomou a operação após ter sido suspenso"
                    break;
                case PowerBroadcastStatus.Suspend:              // O computador está prestes a entrar no modo suspenso
                    // TODO: Implementar case "O computador está prestes a entrar no modo suspenso"
                    break;
                default:
                    break;
            }

            return base.OnPowerEvent(powerStatus); // Executa método base
        }

        /// <summary>
        /// Executa quando um evento de alteração é proveniente de uma sessão do servidor
        /// Host da Sessão da Área de Trabalho Remota.
        /// </summary>
        /// <param name="changeDescription">Uma estrutura que identifica o tipo de alteração.</param>
        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.ConsoleConnect:        // Uma sessão do console foi conectada.
                    // TODO: Implementar case "Uma sessão do console foi conectada."
                    break;
                case SessionChangeReason.ConsoleDisconnect:     // Uma sessão do console foi desconectada.
                    // TODO: Implementar case "Uma sessão do console foi desconectada."
                    break;
                case SessionChangeReason.RemoteConnect:         // Uma sessão remota foi conectada.
                    // TODO: Implementar case "Uma sessão remota foi conectada."
                    break;
                case SessionChangeReason.RemoteDisconnect:      // Uma sessão remota foi desconectada.
                    // TODO: Implementar case "Uma sessão remota foi desconectada."
                    break;
                case SessionChangeReason.SessionLogon:          // Um usuário fez logon em uma sessão.
                    // TODO: Implementar case "Um usuário fez logon em uma sessão."
                    break;
                case SessionChangeReason.SessionLogoff:         // Um usuário fez logoff em uma sessão.
                    // TODO: Implementar case "Um usuário fez logoff em uma sessão."
                    break;
                case SessionChangeReason.SessionLock:           // A sessão foi bloqueada.
                    // TODO: Implementar case "A sessão foi bloqueada."
                    break;
                case SessionChangeReason.SessionUnlock:         // A sessão foi desbloqueada.
                    // TODO: Implementar case "A sessão foi desbloqueada."
                    break;
                case SessionChangeReason.SessionRemoteControl:  // O status de controle remoto de uma sessão foi alterado.
                    // TODO: Implementar case "O status de controle remoto de uma sessão foi alterado."
                    break;
                default:
                    break;
            }

            base.OnSessionChange(changeDescription); // Executa método base
        }

        /// <summary>
        /// Quando implementado em uma classe derivada, é executado quando o sistema é desligado.
        /// Especifica o que deve ocorrer imediatamente antes do desligamento do sistema.
        /// </summary>
        protected override void OnShutdown()
        {
            inst_server.StopServer();    // Pára o Servidor

            inst_server.CloseServer();   // Encerra o servidor

            base.OnShutdown(); // Executa o método base
        }
    }
}
