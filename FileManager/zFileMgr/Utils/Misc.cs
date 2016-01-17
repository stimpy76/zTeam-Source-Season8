using System;
using System.Text;

using System.Windows.Forms;

using System.Text.RegularExpressions;

namespace zFileMgr.Utils
{
    class Misc
    {

        /// <summary>
        /// Highlights specified row in given datagridview
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="row_id"></param>
        /// <param name="Special"></param>
        public static void HighlightRow(DataGridView dgv, int row_id, bool Special)
        {
            if (Special)
            {
                dgv.Rows[row_id].DefaultCellStyle.BackColor = System.Drawing.Color.DeepSkyBlue;
                dgv.Rows[row_id].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
            else
            {
                dgv.Rows[row_id].DefaultCellStyle.BackColor = System.Drawing.Color.SkyBlue;
                dgv.Rows[row_id].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            }
        }

        /// <summary>
        /// Determines if string contains specified value
        /// Allows to search for invariant string matches
        /// </summary>
        /// <param name="str">Source string</param>
        /// <param name="value">String to compare with</param>
        /// <param name="invariant">Enable or disable invariant check</param>
        /// <returns></returns>
        public static bool StrContainsValue(string str, string value, bool invariant)
        {
            return (invariant) ? 
                str.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0
                : str.IndexOf(value, StringComparison.Ordinal) >= 0;
        }

        /// <summary>
        /// Allows to replace substring with ignore case
        /// </summary>
        /// <param name="str">Source string</param>
        /// <param name="old">Old value</param>
        /// <param name="new_str">New value</param>
        /// <param name="IgnoreCase">Enable or disable invariant replacing</param>
        /// <returns></returns>
        public static string StrReplace(string str, string old, string new_str, bool IgnoreCase)
        {
            Regex rx = (IgnoreCase)
                ?
                new Regex(old, RegexOptions.IgnoreCase)
                :
                new Regex(old, RegexOptions.None);

            return rx.Replace(str, new_str);


        }
    }
}
