using DonBigo.Actions;

namespace DonBigo
{
    public abstract class HealthStatus
    {
        public event System.Action OnEndEvent;

        /// <summary> Chamado pelo HealthManager quando o estado começa. </summary>
        public void Start(HealthManager health)
        {
            OnStart(health);
        }
        protected abstract void OnStart(HealthManager health);

        /// <summary> Chamado pelo HealthManager quando o estado termina. </summary>
        public void End(HealthManager health)
        {
            OnEnd(health);
            OnEndEvent?.Invoke();
        }
        protected abstract void OnEnd(HealthManager health);
        
        /// <summary> Chamado pelo HealthManager a cada turno. </summary>
        /// <returns>Retorna true se o status deve acabar. False se deve continuar.</returns>
        public abstract bool Tick(HealthManager health);
        
        /// <summary> Chamado pelo HealthManager após o tick, gera uma ação a ser executada pela entidade. </summary>
        public abstract Action GenAction(HealthManager health);
    }
}