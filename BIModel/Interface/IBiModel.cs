using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TbiesIntfDll;
using BIModel.runtime;
using BiInterface;
using BIModel.Manager;

namespace BIModel.Interface
{
    public interface IBiModel
    {
        string[] GetSupportPlanTable();
        SpecItem[] GetSpecByPlan(string plan);
        #region Get Information
        ConfigParam GetModelParam();
        IBoard GetController(string boardName);
        BoardState GetBoardState(string boardName);
        int GetBoardSeatsCount(string boardName);
        string GetSnByPos(string boardName, int seat);
        string[] GetProductStateOnBoard(string boardName);
        bool IsUnitExist(string sn);
        int GetProgress(string sn);
        Dictionary<string, string> GetProductInformationBySn(string sn);
        List<string> GetSelectedBoard();
        string[] GetUnitParaSet(string sn);
        DataTable FetchUnitData(string sn, int range = 200);
        Dictionary<int, string> GetSnSet(string boardName);
        #endregion

        #region Public Control
        bool Executable();
        void Recover(string boardName);
        void SetBoardEnable(string boardName, string plan);
        void EraseBoard(string boardName);
        void UnitAbort(string boardName, int seat);
        void CheckAllConnection();
        void StartBurnIn();
        void PauseAll();
        void PauseBoard(string boardName);
        void BindingSnOnBoard(string boardName, Dictionary<int, string> snSet);
        void CreateScanResultFile();
        #endregion

        EventHandler ProductsUpdate { get; set; }
        EventHandler BoardStateChanged { get; set; }
        
    }
}
