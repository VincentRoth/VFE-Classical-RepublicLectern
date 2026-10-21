using System;
using UnityEngine;
using Verse;

namespace VFEC_RepublicLectern
{
    // A tab of the lectern window. Other mods add tabs with their own def and worker (no patch needed).
    public class LecternPanelDef : Def
    {
        public Type workerClass;
        public int order;

        private LecternPanelWorker worker;

        public LecternPanelWorker Worker
        {
            get
            {
                if (worker == null)
                {
                    worker = (LecternPanelWorker)Activator.CreateInstance(workerClass);
                    worker.def = this;
                }
                return worker;
            }
        }

        public override System.Collections.Generic.IEnumerable<string> ConfigErrors()
        {
            foreach (string error in base.ConfigErrors()) yield return error;
            if (workerClass == null || !typeof(LecternPanelWorker).IsAssignableFrom(workerClass))
                yield return "workerClass must be a LecternPanelWorker subclass.";
        }
    }

    public abstract class LecternPanelWorker
    {
        public LecternPanelDef def;

        public virtual bool Visible => true;

        public abstract void DoPanel(Rect inRect);

        public virtual void Notify_Closed() { }
    }
}
