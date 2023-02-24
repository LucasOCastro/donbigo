using System.Collections.Generic;
using DonBigo.Actions;
using DonBigo.Rooms;
using UnityEngine;

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
        private const int StrengthMinUpdateInterval = 50;
        
        public Entity Owner { get; }

        private int _minStrength;
        private int _lastStrengthUpdateTurn;
        private void UpdateMinStrength() => _minStrength = Random.Range(50, 100);
        public bool FeelsStrong => Owner.Inventory.CombatPower >= _minStrength;
        
        
        public AIWorker(Entity owner)
        {
            Owner = owner;
            _currentState = new WanderState();
            UpdateMinStrength();
        }
        
        public void EnterVentState(Vent vent)
        {
            if (!Owner.IsVenting)
            {
                Owner.EnterVent(vent);    
            }
            var state = new VentingState(vent);
            _currentState = state;
        }

        private AIState _currentState;
        public Action GetAction()
        {
            if ((TurnManager.CurrentTurn - _lastStrengthUpdateTurn) >= StrengthMinUpdateInterval)
            {
                _lastStrengthUpdateTurn = TurnManager.CurrentTurn;
                UpdateMinStrength();
            }
            
            //Se retorna um novo estado e nenhuma ação, eu quero uma transição instantanea.
            AIState newState;
            Action action;
            do
            {
                newState = _currentState.Tick(this, out action);
                if (newState != null)
                {
                    _currentState = newState;
                }
                
                
            } while (newState != null && action == null);

            return action;
        }
    }
}