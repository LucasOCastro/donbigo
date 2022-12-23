using DonBigo.Actions;

namespace DonBigo.AI
{
    /*se 'distraída' procure 'cômodo'
    se 'distraida' e (viu jogador) ou (outros gatilhos) fique 'alerta'
    se 'alerta' e (tem bons itens) procure jogador
    se 'alerta' e (não tem bons itens) procure itens
    se 'alerta' e (achou o jogador) fique 'agressiva'
    se 'alerta' e (muito tempo se passou) fique 'distraída'
    se 'agressiva' e (tem bons itens) ataque o jogador
    se 'agressiva' e (não tem bons itens) fuja do jogador e fique 'alerta'
    •procure:  escolha uma porta aleatoriamente e vá até ela, até estar no cômodo procurado ou ver o item ou jogador procurado
    •tem bons itens: cada item tem uma pontuação, e a pontuação total mínima considerada boa deve variar aleatoriamente para não ficar repetitivo
    •ataque o jogador: use os itens que tem, com alguma prioridade ou não 
    muito tempo se passou: uma quantidade de jogadas (pode ser aleatória)
    •fuja do jogador: A* até a porta mais distante do jogador, mas sem se aproximar dele uma distância mínima
    Bônus: criar uma tile especial invisível que a Phantonette pode visitar quando está procurando algo e colocar em lugares que ela não iria só pela busca porta a porta*/
    public class AIWorker
    {
        public Entity Owner { get; }

        public AIWorker(Entity owner)
        {
            Owner = owner;
            _currentState = new WanderState();
        }
        
        private AIState _currentState;
        public Action GetAction()
        {
            //Se retorna um novo estado e nenhuma ação, eu quero uma transição instantanea.
            AIState newState;
            Action action;
            do
            {
                newState = _currentState.Tick(Owner, out action);
                if (newState != null)
                {
                    _currentState = newState;
                }
            } while (newState != null && action == null);

            return action;
        }
    }
}