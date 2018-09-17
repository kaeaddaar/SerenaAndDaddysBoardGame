using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appGameBoardTest.Entities
{
    public class Decisions
    {
        Stopwatch watch;
        private List<IDecision> _Decisions;

        public Decisions()
        {
            watch = new Stopwatch();
            _Decisions = new List<IDecision>();
            watch.Start();
        }

        public Decisions(IEnumerable<IDecision> ListToStartFrom)
        {
            watch = new Stopwatch();
            _Decisions = new List<IDecision>(ListToStartFrom);
            watch.Start();
        }

        public void AddDecision(IDecision DecisionToConsider)
        {
            _Decisions.Add(DecisionToConsider);
        }

        public void LoadAvailableDecisions()
        {
            foreach (var d in _Decisions)
            {
                if (d.FireDecisionIfTime(watch))
                {
                    // could take another action. Note: IsTimeToDecide activates a decision
                }
                else
                {
                    // could take an action if IsNotTimeToDecide
                }
            }
        }

    }


    public interface IDecision
    {
        bool FireDecisionIfTime(Stopwatch watch);
        void SetActionToTake(Func<int> ActionToTake);
    }

    public class HasBeenLongEnough : IDecision
    {
        private long msToDecide;
        private long lastDecision;
        private Func<int> Execute;

        public HasBeenLongEnough(long MillisecondsToDecisionTime)
        {
            if (MillisecondsToDecisionTime <= 0)
            {
                throw new Exception("HasBeenLongEnough requires a MillisecondsToDecisionTime > 0");
            }
            msToDecide = MillisecondsToDecisionTime;
        }

        public bool FireDecisionIfTime(Stopwatch watch)
        {
            long ms = watch.ElapsedMilliseconds;
            long currDecision = ms / msToDecide;
            if (currDecision > lastDecision)
            {
                lastDecision = currDecision;
                if (Execute != null) { Execute(); }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetActionToTake(Func<int> ActionToTake)
        {
            Execute = ActionToTake;
        }
    }

}
