
using BIModel.Interface;
using BIModel.Manager;
using BILib;


namespace BIModel
{
    public class BiModelFactory
    {
        public static IBiModel GetBiModel(log4net.ILog logger, IDatabaseService dbStore, ConfigParam param)
        {
           
            return new BIModelTOSA(param, logger, dbStore);
        }
        public static IMesOperator GetMesOperator()
        {
            return MesOperator.GetInstance();
        }
        public static IPosMapScheme CreateIPosMapScheme(IDatabaseService dbStore)
        {
            return  PosMapScheme.Inst(dbStore);
        }
    }
}
