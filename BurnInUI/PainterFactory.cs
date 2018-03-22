using System.Collections.Generic;
using System.Drawing;
using ProgressODoom;

namespace BurnInUI
{
    class PainterFactory
    {

        public static PlainBackgroundPainter GetBoardStatePainter(string state)
        {
            string[] stateList = new string[] { "UNSELECTED", "SELECTED", "LOADED", "READY", "RUNNING", "DONE", "CONFLICT" };
            Color[] colorList = new Color[] { Color.Gray, Color.LightSkyBlue, Color.LightSalmon, Color.Orange, Color.LightGreen, Color.Green, Color.Red };
            Dictionary<string,Color> dict = new Dictionary<string, Color>();
            for (int i = 0; i < stateList.Length; i++)
            {
                dict[stateList[i]] = colorList[i];
            }
            return new PlainBackgroundPainter(dict[state]);
        }

        public static PlainBorderPainter GetPlainBoarderPainter(string state)
        {
            Dictionary<string, Color> dict = new Dictionary<string, Color>() { { "ACTIVE",Color.Red}, { "UNACTIVE",Color.Transparent}};
            PlainBorderPainter ret = new PlainBorderPainter(dict[state]);
            ret.RoundedCorners = false;
            ret.Style = PlainBorderPainter.PlainBorderStyle.Flat;
            return ret;
        }

        public static PlainBackgroundPainter GetProductStatePainter(string state)
        {
            string[] stateList = new string[] { "EMPTY", "READY", "BURNIN", "PAUSE", "PASS", "FAIL","REWORK"};
            Color[] colorList = new Color[] { Color.LightSkyBlue, Color.Orange, Color.LightGreen, Color.Yellow, Color.Lime, Color.Red,Color.Tomato };
            Dictionary<string, Color> dict = new Dictionary<string, Color>();
            for (int i = 0; i < stateList.Length; i++)
            {
                dict[stateList[i]] = colorList[i];
            }
            return new PlainBackgroundPainter(dict[state]);
        }

        public static PlainProgressPainter GetProgressPainter(string state)
        {
            string[] stateList = new string[] {"BURNIN","FAIL"};
            Color[] colorList = new Color[] {Color.Lime,Color.Red};
            Dictionary<string, Color> dict = new Dictionary<string, Color>();
            for (int i = 0; i < stateList.Length; i++)
            {
                dict[stateList[i]] = colorList[i];
            }
            PlainProgressPainter ret = new PlainProgressPainter();
            ret.Color = dict[state];
            ret.GlossPainter = null;
            ret.LeadingEdge = System.Drawing.Color.Transparent;
            ret.ProgressBorderPainter = null;
            return ret;
        }
        
    }
}
