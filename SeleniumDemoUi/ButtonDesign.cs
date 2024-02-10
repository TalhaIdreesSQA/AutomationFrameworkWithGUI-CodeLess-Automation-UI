using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumDemoUi
{
    internal class ButtonDesign
    {
        public void GenerateFieldsButton_MouseEnter(object sender, EventArgs e)
        {
            // Change background color to indicate hover
            ((System.Windows.Forms.Button)sender).BackColor = System.Drawing.Color.LightSeaGreen;
        }

        // MouseLeave event handler for Generate Fields Button
        public void GenerateFieldsButton_MouseLeave(object sender, EventArgs e)
        {
            // Restore initial background color when mouse leaves
            ((System.Windows.Forms.Button)sender).BackColor = System.Drawing.Color.GhostWhite;
        }

        
    }
}
