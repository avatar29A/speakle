using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqub.Speckle.GUI.Experiment;

namespace Hqub.Speckle.Core
{
    public class Experiment : IExperiment
    {
        private Experiment()
        {

        }

        #region Public Properties

        #endregion


        #region Static Interfaces

        private static Experiment _instance;
        private static Experiment Create()
        {
            return new Experiment();
        }

        public static Experiment Get(bool isForseCreate=false)
        {
            if (isForseCreate)
                return _instance = Create();

            return _instance = _instance ?? Create();
        }

        #endregion

    }
}
