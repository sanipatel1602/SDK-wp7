using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace WindowsFormsApplication1
{
    public partial class ChangeCSS : Form
    {
        public ChangeCSS()
        {
            InitializeComponent();
        }

        public ProjectEntity project = new ProjectEntity();
        bool proverka = false;

        private string ChoiseOfColor()
        {
            string choise = "";
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                int rvalue = Convert.ToInt32(colorDialog1.Color.R.ToString(), 10);
                int gvalue = Convert.ToInt32(colorDialog1.Color.G.ToString(), 10);
                int bvalue = Convert.ToInt32(colorDialog1.Color.B.ToString(), 10);
                string r = Convert.ToString(rvalue, 16), g = Convert.ToString(gvalue, 16), b = Convert.ToString(bvalue, 16);
                if (r == "0")
                    r = "00";
                if (g == "0")
                    g = "00";
                if (b == "0")
                    b = "00";

                choise = "#" + r + g + b;
                return choise;
            }
            return choise;
        }

        private void button1_Click(object sender, EventArgs e)
        {   
             ParseCSSTOC();
             if (proverka == true)
             {
                 SaveOtkatStyle();
                 otkats.Clear();
                 if (!Directory.Exists("C:\\Projects\\CSS\\"))
                     Directory.CreateDirectory("C:\\Projects\\CSS\\");
                 if (!Directory.Exists("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\"))
                     Directory.CreateDirectory("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\");
                 method(new DirectoryInfo("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\"));
                 i = otkats.Count() - 1;
                 if (i == (otkats.Count() - 1))
                     button25.Enabled = false;
                 else button25.Enabled = true;
                 if (i > 0)
                     button24.Enabled = true;
             }
             
        }

        private string ParsingCss(string[] strings, string temp, string search)
        {
            bool check = false;
            foreach (string str in strings)
            {
                if (str == search)
                    check = true;
                if (str.Contains("}") && check == true)
                {
                    temp += str;
                    check = false;
                }
                if (check)
                {
                    temp += str;
                    temp += "\r\n";
                }
            }
            return temp;
        }

        private string ParsingPCss(string[] strings, string temp, string search)
        {
            bool check = false;
            foreach (string str in strings)
            {
                if (str == search)
                    check = true;
                if (str.Contains("}") && check == true)
                {
                    temp += str;
                    check = false;
                }
                if (check)
                {
                    temp += str;
                    temp += "\r\n";
                }
            }
            return temp;
        }

        private void ParseCSSTOC()
        {
            bool check;
            OpenFileDialog dialog = new OpenFileDialog();
            if (File.Exists(project.ProjectSource + @"\" + project.ProjectName + "_ProjectDir" + @"\" + project.ProjectName + @"\" + "TOC.css"))
            {
                proverka = true;
                string path = project.ProjectSource + @"\" + project.ProjectName + "_ProjectDir" + @"\" + project.ProjectName + @"\" + "TOC.css";
                string temp = "";
                string[] strings = File.ReadAllLines(path, Encoding.UTF8);
                string originaltext = File.ReadAllText(path, Encoding.UTF8);
                check = false;

                //body - список дерево
                temp = ParsingCss(strings, temp, "body");
                string temp_new = "body" + "\r\n" + "{" + "\r\n" + "font-family: " + tree_combo_shrift.Text + ";" + "\r\n" + "font-size: " + tree_txtbox_size.Text.Replace(",", ".") + "px;" + "\r\n" + "background-color: #6699CC;" + "\r\n" + "color: White;" + "\r\n" + "overflow: " + tree_txtbox_hide.Text + ";" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                // tree - список дерева
                temp = ParsingCss(strings, temp, ".Tree");
                temp_new = ".Tree" + "\r\n" + "{" + "\r\n" + "background-color: " + tree_txtbox_color.Text + ";" + "\r\n" + "color: Black;" + "\r\n" + "width: 300px;" + "\r\n" + "overflow: auto;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                // a.SelectedNode - выбранный node
                temp = ParsingCss(strings, temp, "a.SelectedNode");
                temp_new = "a.SelectedNode" + "\r\n" + "{" + "\r\n" + "background-color: " + node_txtbox_color.Text + ";" + "\r\n" + "padding: " + node_txtbox_borders.Text + " " + node_txtbox_borders2.Text.Replace(",", ".") + "px #999999;" + "\r\n" + "border: " + node_txtbox_margin_top.Text.Replace(",", ".") + "px " + node_txtbox_margin_right.Text.Replace(",", ".") + "px " + node_txtbox_margin_bottom.Text.Replace(",", ".") + "px " + node_txtbox_margin_left.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                // img - иконки +\-\*
                temp = ParsingCss(strings, temp, "img");
                temp_new = "img" + "\r\n" + "{" + "\r\n" + "border: " + icons_txtbox_borders.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-left: " + icons_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-right: " + icons_txtbox_marginright.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-bottom: " + icons_txtbox_marginbottom.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-top: " + icons_txtbox_margintop.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                // Selected/Unselected node - ноды
                temp = ParsingCss(strings, temp, "a.SelectedNode, a.UnselectedNode");
                temp_new = "a.SelectedNode, a.UnselectedNode" + "\r\n" + "{" + "\r\n" + "color: " + selectnode_txtbox_shriftcolor.Text + ";" + "\r\n" + "text-decoration: none;" + "\r\n" + "padding: " + selectnode_txtbox_margintop.Text.Replace(",", ".") + "px " + selectnode_txtbox_marginright.Text.Replace(",", ".") + "px " + selectnode_txtbox_marginbottom.Text.Replace(",", ".") + "px " + selectnode_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "white-space: " + selectnode_txtbox_raps.Text + ";" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";


                File.WriteAllText(path, originaltext);
                MessageBox.Show("Изменения приняты");
            }
            else
            {
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    proverka = true;
                    string path = dialog.FileName;
                    string temp = "";
                    string[] strings = File.ReadAllLines(path, Encoding.UTF8);
                    string originaltext = File.ReadAllText(path, Encoding.UTF8);
                    check = false;

                    //body - список дерево
                    temp = ParsingCss(strings, temp, "body");
                    string temp_new = "body" + "\r\n" + "{" + "\r\n" + "font-family: " + tree_combo_shrift.Text + ";" + "\r\n" + "font-size: " + tree_txtbox_size.Text.Replace(",", ".") + "px;" + "\r\n" + "background-color: #6699CC;" + "\r\n" + "color: White;" + "\r\n" + "overflow: " + tree_txtbox_hide.Text + ";" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    // tree - список дерева
                    temp = ParsingCss(strings, temp, ".Tree");
                    temp_new = ".Tree" + "\r\n" + "{" + "\r\n" + "background-color: " + tree_txtbox_color.Text + ";" + "\r\n" + "color: Black;" + "\r\n" + "width: 300px;" + "\r\n" + "overflow: auto;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    // a.SelectedNode - выбранный node
                    temp = ParsingCss(strings, temp, "a.SelectedNode");
                    temp_new = "a.SelectedNode" + "\r\n" + "{" + "\r\n" + "background-color: " + node_txtbox_color.Text + ";" + "\r\n" + "padding: " + node_txtbox_borders.Text + " " + node_txtbox_borders2.Text.Replace(",", ".") + "px #999999;" + "\r\n" + "border: " + node_txtbox_margin_top.Text.Replace(",", ".") + "px " + node_txtbox_margin_right.Text.Replace(",", ".") + "px " + node_txtbox_margin_bottom.Text.Replace(",", ".") + "px " + node_txtbox_margin_left.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    // img - иконки +\-\*
                    temp = ParsingCss(strings, temp, "img");
                    temp_new = "img" + "\r\n" + "{" + "\r\n" + "border: " + icons_txtbox_borders.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-left: " + icons_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-right: " + icons_txtbox_marginright.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-bottom: " + icons_txtbox_marginbottom.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-top: " + icons_txtbox_margintop.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    // Selected/Unselected node - ноды
                    temp = ParsingCss(strings, temp, "a.SelectedNode, a.UnselectedNode");
                    temp_new = "a.SelectedNode, a.UnselectedNode" + "\r\n" + "{" + "\r\n" + "color: " + selectnode_txtbox_shriftcolor.Text + ";" + "\r\n" + "text-decoration: none;" + "\r\n" + "padding: " + selectnode_txtbox_margintop.Text.Replace(",", ".") + "px " + selectnode_txtbox_marginright.Text.Replace(",", ".") + "px " + selectnode_txtbox_marginbottom.Text.Replace(",", ".") + "px " + selectnode_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "white-space: " + selectnode_txtbox_raps.Text + ";" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";


                    File.WriteAllText(path, originaltext);
                    MessageBox.Show("Изменения приняты");
                }
                else
                {
                    proverka = false;
                    MessageBox.Show("Файл не найден");
                }
            }
                
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tree_txtbox_color.Text = ChoiseOfColor();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            node_txtbox_color.Text = ChoiseOfColor();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            selectnode_txtbox_shriftcolor.Text = ChoiseOfColor();
        }


        //http://rim89.wordpress.com/2010/02/05/-px-pt-em/
        private void ParseCSSPresentation()
        {
            bool check;
            OpenFileDialog dialog = new OpenFileDialog();
            if (File.Exists(project.ProjectSource + @"\" + project.ProjectName + "_ProjectDir" + @"\" + project.ProjectName + @"\styles\" + "Presentation.css"))
            {
                string path = project.ProjectSource + @"\" + project.ProjectName + "_ProjectDir" + @"\" + project.ProjectName + @"\styles\" + "Presentation.css";
                string temp = "";
                string[] strings = File.ReadAllLines(path, Encoding.UTF8);
                string originaltext = File.ReadAllText(path, Encoding.UTF8);
                check = false;

                //div#header table #bottomTable - верхняя таблица    саксед
                temp = ParsingCss(strings, temp, "div#header table#bottomTable");
                string temp_new = "div#header table#bottomTable" + "\r\n" + "{" + "\r\n" + "border-top-color: " + tablerow2td_txtbox_border3.Text + ";" + "\r\n" + "border-top-style: " + tablerow2td_txtbox_border2.Text + ";" + "\r\n" + "border-top-width: " + tablerow2td_txtbox_border.Text.Replace(",", ".") + "px;" + "\r\n" + "text-align: left" + ";" + "\r\n" + "padding: " + tablerow2td_txtbox_margintop.Text.Replace(",", ".") + "px " + tablerow2td_txtbox_marginright.Text.Replace(",", ".") + "px " + tablerow2td_txtbox_marginbottom.Text.Replace(",", ".") + "px " + tablerow2td_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //div#header - верхняя таблица                       саксед
                temp = ParsingCss(strings, temp, "div#header");
                temp_new = "div#header" + "\r\n" + "{" + "\r\n" + "font-family: " + tablerow2td_combobox_shrift.Text + ";" + "\r\n" + "background-color: #FFFFFF;" + "\r\n" + "padding-top: 0;" + "\r\n" + "width: 100%;" + "\r\n" + "\r\n" + "padding-bottom: 0;" + "\r\n" + "padding-left: 0;" + "\r\n" + "padding-right: 0;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //span#nsrTitle - верхняя таблица                    саксед
                temp = ParsingCss(strings, temp, "span#nsrTitle");
                temp_new = "span#nsrTitle" + "\r\n" + "{" + "\r\n" + "color: " + header_txtbox_textcolor.Text + ";" + "\r\n" + "font-size: " + header_txtbox_textsize.Text + "%;" + "\r\n" + "font-family: " + header_combobox_shrift.Text + ";" + "\r\n" + "font-weight: 400;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //div#header table#toptable td - верхняя таблица     саксед
                temp = ParsingCss(strings, temp, "div#header table#toptable td");
                temp_new = "div#header table#toptable td" + "\r\n" + "{" + "\r\n" + "padding: " + toptabletd_txtbox_margintop.Text.Replace(",", ".") + "px " + toptabletd_txtbox_marginright.Text.Replace(",", ".") + "px " + toptabletd_txtbox_marginbottom.Text.Replace(",", ".") + "px " + toptabletd_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //div#header table td - верхняя таблица              саксед          
                temp = ParsingCss(strings, temp, "div#header table td");
                temp_new = "div#header table td" + "\r\n" + "{" + "\r\n" + "color: #0000FF;" + "\r\n" + "font-size: " + toptable_txtbox_size.Text + "%;" + "\r\n" + "margin-top:	0;" + "\r\n" + "margin-bottom: 0;" + "\r\n" + "padding-right: 20;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //span#runningHeaderText - верхняя таблица          саксед
                temp = ParsingCss(strings, temp, "span#runningHeaderText");
                temp_new = "span#runningHeaderText" + "\r\n" + "{" + "\r\n" + "color: " + tablerow1td_txtbox_shriftcolor.Text + ";" + "\r\n" + "font-size: " + tablerow1td_txtbox_textsize.Text + "%;" + "\r\n" + "padding:" + tablerow1td_txtbox_margintop.Text.Replace(",", ".") + "px " + tablerow1td_txtbox_marginright.Text.Replace(",", ".") + "px " + tablerow1td_txtbox_marginbottom.Text.Replace(",", ".") + "px " + tablerow1td_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //div#header table tr#headerTableRow3 td - верхняя таблица  
                temp = ParsingCss(strings, temp, "div#header table tr#headerTableRow3 td");
                temp_new = "div#header table tr#headerTableRow3 td" + "\r\n" + "{" + "\r\n" + "padding: " + tablerow3td_txtbox_margintop.Text.Replace(",", ".") + "px " + tablerow3td_txtbox_marginright.Text.Replace(",", ".") + "px " + tablerow3td_txtbox_marginbottom.Text.Replace(",", ".") + "px " + tablerow3td_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //h1.heading - средняя таблица header    - ?
                temp = ParsingCss(strings, temp, "h1.heading");
                temp_new = "h1.heading" + "\r\n" + "{" + "\r\n" + "color: " + h1_txtbox_textcolor.Text + ";" + "\r\n" + "font-size: " + h1_txtbox_textsize.Text + "%;" + "\r\n" + "font-family: " + h1_combobox_shrift.Text + ";" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //img.toggle - средняя таблица - иконка рядом с headerom
                temp = ParsingCss(strings, temp, "img.toggle");
                temp_new = "img.toggle" + "\r\n" + "{" + "\r\n" + "border: " + headericons_txtbox_border.Text.Replace(",", ".") + "px " + headericons_txtbox_border2.Text + ";" + "\r\n" + "margin: " + headericons_txtbox_margintop.Text.Replace(",", ".") + "px " + headericons_txtbox_marginright.Text.Replace(",", ".") + "px " + headericons_txtbox_marginbottom.Text.Replace(",", ".") + "px " + headericons_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //img.toggle - средняя таблица header
                temp = ParsingCss(strings, temp, ".heading");
                temp_new = ".heading" + "\r\n" + "{" + "\r\n" + "margin-left: " + h1_txtbox_marginleft.Text.Replace(",", ".") + ";" + "\r\n" + "margin-right: " + h1_txtbox_marginright.Text.Replace(",", ".") + ";" + "\r\n" + "margin-bottom: " + h1_txtbox_marginbottom.Text.Replace(",", ".") + ";" + "\r\n" + "margin-top: " + h1_txtbox_margintop.Text.Replace(",", ".") + ";" + "\r\n" + "font-weight: " + h1_txtbox_weight.Text + ";" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //p { - Namespace\Assembly
                temp = ParsingCss(strings, temp, "p {");
                temp_new = "p {" + "\r\n" + "margin-top: " + p_txtbox_margintop.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-bottom: " + p_txtbox_marginbottom.Text.Replace(",", ".") + "px;" /*+ "\r\n" + "margin-right: " + p_txtbox_marginright.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-left: " + p_txtbox_marginleft.Text.Replace(",", ".") + "px;"*/ + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //html>body #mainBody - текст внутри тела
                temp = ParsingCss(strings, temp, "html>body #mainBody");
                temp_new = "html>body #mainBody" + "\r\n" + "{" + "\r\n" + "margin-right: " + body_txtbox_marginright.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-left: " + body_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-top: " + body_txtbox_margintop.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-bottom: " + body_txtbox_marginbottom.Text.Replace(",", ".") + "px;" + "\r\n" + "font-size: " + body_txtbox_textsize.Text + "%;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                // div#mainSection table th, div#mainSectionMHS table th - заголовок(C#) в таблице классов, th
                temp = ParsingCss(strings, temp, "div#mainSection table th, div#mainSectionMHS table th");
                temp_new = "div#mainSection table th, div#mainSectionMHS table th" + "\r\n" + "{" + "\r\n" + "background-color: " + body2_txtbox_color.Text + ";" + "\r\n" + "border-bottom: " + body2_txtbox_borderbottom.Text.Replace(",", ".") + "px " + body2_txtbox_borderbottom2.Text + " " + body2_txtbox_borderbottom3.Text + ";" + "\r\n" + "border-left: " + body2_txtbox_borderleft.Text.Replace(",", ".") + "px " + body2_txtbox_borderleft2.Text + " " + body2_txtbox_borderleft3.Text + " ;" + "\r\n" + "color: #000066;" + "\r\n" + "padding-left: " + body2_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-right: " + body2_txtbox_marginright.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-bottom: " + body2_txtbox_marginbottom.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-top: " + body2_txtbox_margintop.Text.Replace(",", ".") + "px;" + "\r\n" + "text-align: " + body2_txtbox_allign.Text + ";" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //html>body div.section - секции внизу (нижние таблицы), их расположение
                temp = ParsingCss(strings, temp, "html>body div.section");
                temp_new = "html>body div.section" + "\r\n" + "{" + "\r\n" + "margin-left: 0" + ";" + "\r\n" + "padding-top: " + sections_txtbox_margintop.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-bottom: " + sections_txtbox_marginbottom.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-left: " + sections_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-right: " + sections_txtbox_marginright.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //div#mainSection table td, div#mainSectionMHS table td - секции внизу (нижние таблицы), td
                temp = ParsingCss(strings, temp, "div#mainSection table td, div#mainSectionMHS table td");
                temp_new = "div#mainSection table td, div#mainSectionMHS table td" + "\r\n" + "{" + "\r\n" + "background-color: " + bodytd_txtbox_color.Text + ";" + "\r\n" + "border-bottom: " + bodytd_txtbox_border.Text.Replace(",", ".") + "px " + bodytd_txtbox_border2.Text + " " + bodytd_txtbox_border3.Text + ";" + "\r\n" + "border-left: " + bodytd_txtbox_borderleft.Text.Replace(",", ".") + "px " + bodytd_txtbox_borderleft2.Text + " " + bodytd_txtbox_borderleft3.Text + " " + ";" + "\r\n" + "padding-left: " + bodytd_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-right: " + bodytd_txtbox_borderright.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-top: " + bodytd_txtbox_bordertop.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-bottom: " + bodytd_txtbox_borderbottom.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";
                //body  - основной текст на странице, некоторые заголовки пунктов
                temp = ParsingCss(strings, temp, "body");
                temp_new = "body" + "\r\n" + "{" + "\r\n" + "background: " + bodytext_txt_fonecolor.Text + ";" + "\r\n" + "color: " + bodytext_txt_colortext.Text + ";" + "\r\n" + "font-family: " + bodytext_combobox_textstyle.Text + ";" + "\r\n" + "font-size: " + bodytext_txt_text_size.Text + "%;" + "\r\n" + "font-style: normal;\r\n font-weight: normal;\r\nmargin-top:	0;\r\nmargin-bottom:	0;\r\nmargin-left:	0;\r\nmargin-right:	0;\r\nwidth:	100%;\r\n " + "}" + "\r\n";
                originaltext = originaltext.Replace(temp, temp_new);
                temp = ""; temp_new = "";

                File.WriteAllText(path, originaltext);
                MessageBox.Show("Изменения приняты");
            }
            else
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string path = dialog.FileName;
                    string temp = "";
                    string[] strings = File.ReadAllLines(path, Encoding.UTF8);
                    string originaltext = File.ReadAllText(path, Encoding.UTF8);
                    check = false;

                    //div#header table #bottomTable - верхняя таблица    саксед
                    temp = ParsingCss(strings, temp, "div#header table#bottomTable");
                    string temp_new = "div#header table#bottomTable" + "\r\n" + "{" + "\r\n" + "border-top-color: " + tablerow2td_txtbox_border3.Text + ";" + "\r\n" + "border-top-style: " + tablerow2td_txtbox_border2.Text + ";" + "\r\n" + "border-top-width: " + tablerow2td_txtbox_border.Text.Replace(",", ".") + "px;" + "\r\n" + "text-align: left" + ";" + "\r\n" + "padding: " + tablerow2td_txtbox_margintop.Text.Replace(",", ".") + "px " + tablerow2td_txtbox_marginright.Text.Replace(",", ".") + "px " + tablerow2td_txtbox_marginbottom.Text.Replace(",", ".") + "px " + tablerow2td_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //div#header - верхняя таблица                       саксед
                    temp = ParsingCss(strings, temp, "div#header");
                    temp_new = "div#header" + "\r\n" + "{" + "\r\n" + "font-family: " + tablerow2td_combobox_shrift.Text + ";" + "\r\n" + "background-color: #FFFFFF;" + "\r\n" + "padding-top: 0;" + "\r\n" + "width: 100%;" + "\r\n" + "\r\n" + "padding-bottom: 0;" + "\r\n" + "padding-left: 0;" + "\r\n" + "padding-right: 0;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //span#nsrTitle - верхняя таблица                    саксед
                    temp = ParsingCss(strings, temp, "span#nsrTitle");
                    temp_new = "span#nsrTitle" + "\r\n" + "{" + "\r\n" + "color: " + header_txtbox_textcolor.Text + ";" + "\r\n" + "font-size: " + header_txtbox_textsize.Text + "%;" + "\r\n" + "font-family: " + header_combobox_shrift.Text + ";" + "\r\n" + "font-weight: 400;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //div#header table#toptable td - верхняя таблица     саксед
                    temp = ParsingCss(strings, temp, "div#header table#toptable td");
                    temp_new = "div#header table#toptable td" + "\r\n" + "{" + "\r\n" + "padding: " + toptabletd_txtbox_margintop.Text.Replace(",", ".") + "px " + toptabletd_txtbox_marginright.Text.Replace(",", ".") + "px " + toptabletd_txtbox_marginbottom.Text.Replace(",", ".") + "px " + toptabletd_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //div#header table td - верхняя таблица              саксед          
                    temp = ParsingCss(strings, temp, "div#header table td");
                    temp_new = "div#header table td" + "\r\n" + "{" + "\r\n" + "color: #0000FF;" + "\r\n" + "font-size: " + toptable_txtbox_size.Text + "%;" + "\r\n" + "margin-top:	0;" + "\r\n" + "margin-bottom: 0;" + "\r\n" + "padding-right: 20;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //span#runningHeaderText - верхняя таблица          саксед
                    temp = ParsingCss(strings, temp, "span#runningHeaderText");
                    temp_new = "span#runningHeaderText" + "\r\n" + "{" + "\r\n" + "color: " + tablerow1td_txtbox_shriftcolor.Text + ";" + "\r\n" + "font-size: " + tablerow1td_txtbox_textsize.Text + "%;" + "\r\n" + "padding:" + tablerow1td_txtbox_margintop.Text.Replace(",", ".") + "px " + tablerow1td_txtbox_marginright.Text.Replace(",", ".") + "px " + tablerow1td_txtbox_marginbottom.Text.Replace(",", ".") + "px " + tablerow1td_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //div#header table tr#headerTableRow3 td - верхняя таблица  
                    temp = ParsingCss(strings, temp, "div#header table tr#headerTableRow3 td");
                    temp_new = "div#header table tr#headerTableRow3 td" + "\r\n" + "{" + "\r\n" + "padding: " + tablerow3td_txtbox_margintop.Text.Replace(",", ".") + "px " + tablerow3td_txtbox_marginright.Text.Replace(",", ".") + "px " + tablerow3td_txtbox_marginbottom.Text.Replace(",", ".") + "px " + tablerow3td_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //h1.heading - средняя таблица header    - ?
                    temp = ParsingCss(strings, temp, "h1.heading");
                    temp_new = "h1.heading" + "\r\n" + "{" + "\r\n" + "color: " + h1_txtbox_textcolor.Text + ";" + "\r\n" + "font-size: " + h1_txtbox_textsize.Text + "%;" + "\r\n" + "font-family: " + h1_combobox_shrift.Text + ";" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //img.toggle - средняя таблица - иконка рядом с headerom
                    temp = ParsingCss(strings, temp, "img.toggle");
                    temp_new = "img.toggle" + "\r\n" + "{" + "\r\n" + "border: " + headericons_txtbox_border.Text.Replace(",", ".") + "px " + headericons_txtbox_border2.Text + ";" + "\r\n" + "margin: " + headericons_txtbox_margintop.Text.Replace(",", ".") + "px " + headericons_txtbox_marginright.Text.Replace(",", ".") + "px " + headericons_txtbox_marginbottom.Text.Replace(",", ".") + "px " + headericons_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //img.toggle - средняя таблица header
                    temp = ParsingCss(strings, temp, ".heading");
                    temp_new = ".heading" + "\r\n" + "{" + "\r\n" + "margin-left: " + h1_txtbox_marginleft.Text.Replace(",", ".") + ";" + "\r\n" + "margin-right: " + h1_txtbox_marginright.Text.Replace(",", ".") + ";" + "\r\n" + "margin-bottom: " + h1_txtbox_marginbottom.Text.Replace(",", ".") + ";" + "\r\n" + "margin-top: " + h1_txtbox_margintop.Text.Replace(",", ".") + ";" + "\r\n" + "font-weight: " + h1_txtbox_weight.Text + ";" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //p { - Namespace\Assembly
                    temp = ParsingCss(strings, temp, "p ");
                    temp_new = "p \r\n{" + "\r\n" + "margin-top: " + p_txtbox_margintop.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-bottom: " + p_txtbox_marginbottom.Text.Replace(",", ".") + "px;" /*+ "\r\n" + "margin-right: " + p_txtbox_marginright.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-left: " + p_txtbox_marginleft.Text.Replace(",", ".") + "px;"*/ + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //html>body #mainBody - текст внутри тела
                    temp = ParsingCss(strings, temp, "html>body #mainBody");
                    temp_new = "html>body #mainBody" + "\r\n" + "{" + "\r\n" + "margin-right: " + body_txtbox_marginright.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-left: " + body_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-top: " + body_txtbox_margintop.Text.Replace(",", ".") + "px;" + "\r\n" + "margin-bottom: " + body_txtbox_marginbottom.Text.Replace(",", ".") + "px;" + "\r\n" + "font-size: " + body_txtbox_textsize.Text + "%;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    // div#mainSection table th, div#mainSectionMHS table th - заголовок(C#) в таблице классов, th
                    temp = ParsingCss(strings, temp, "div#mainSection table th, div#mainSectionMHS table th");
                    temp_new = "div#mainSection table th, div#mainSectionMHS table th" + "\r\n" + "{" + "\r\n" + "background-color: " + body2_txtbox_color.Text + ";" + "\r\n" + "border-bottom: " + body2_txtbox_borderbottom.Text.Replace(",", ".") + "px " + body2_txtbox_borderbottom2.Text + " " + body2_txtbox_borderbottom3.Text + ";" + "\r\n" + "border-left: " + body2_txtbox_borderleft.Text.Replace(",", ".") + "px " + body2_txtbox_borderleft2.Text + " " + body2_txtbox_borderleft3.Text + " ;" + "\r\n" + "color: #000066;" + "\r\n" + "padding-left: " + body2_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-right: " + body2_txtbox_marginright.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-bottom: " + body2_txtbox_marginbottom.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-top: " + body2_txtbox_margintop.Text.Replace(",", ".") + "px;" + "\r\n" + "text-align: " + body2_txtbox_allign.Text + ";" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //html>body div.section - секции внизу (нижние таблицы), их расположение
                    temp = ParsingCss(strings, temp, "html>body div.section");
                    temp_new = "html>body div.section" + "\r\n" + "{" + "\r\n" + "margin-left: 0" + ";" + "\r\n" + "padding-top: " + sections_txtbox_margintop.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-bottom: " + sections_txtbox_marginbottom.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-left: " + sections_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-right: " + sections_txtbox_marginright.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //div#mainSection table td, div#mainSectionMHS table td - секции внизу (нижние таблицы), td
                    temp = ParsingCss(strings, temp, "div#mainSection table td, div#mainSectionMHS table td");
                    temp_new = "div#mainSection table td, div#mainSectionMHS table td" + "\r\n" + "{" + "\r\n" + "background-color: " + bodytd_txtbox_color.Text + ";" + "\r\n" + "border-bottom: " + bodytd_txtbox_border.Text.Replace(",", ".") + "px " + bodytd_txtbox_border2.Text + " " + bodytd_txtbox_border3.Text + ";" + "\r\n" + "border-left: " + bodytd_txtbox_borderleft.Text.Replace(",", ".") + "px " + bodytd_txtbox_borderleft2.Text + " " + bodytd_txtbox_borderleft3.Text + " " + ";" + "\r\n" + "padding-left: " + bodytd_txtbox_marginleft.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-right: " + bodytd_txtbox_borderright.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-top: " + bodytd_txtbox_bordertop.Text.Replace(",", ".") + "px;" + "\r\n" + "padding-bottom: " + bodytd_txtbox_borderbottom.Text.Replace(",", ".") + "px;" + "\r\n" + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";
                    //body  - основной текст на странице, некоторые заголовки пунктов
                    temp = ParsingCss(strings, temp, "body");
                    temp_new = "body" + "\r\n" + "{" + "\r\n" + "background: " + bodytext_txt_fonecolor.Text + ";" + "\r\n" + "color: " + bodytext_txt_colortext.Text + ";" + "\r\n" + "font-family: " + bodytext_combobox_textstyle.Text + ";" + "\r\n" + "font-size: " + bodytext_txt_text_size.Text + "%;" + "\r\n" + "font-style: normal;\r\n font-weight: normal;\r\nmargin-top:	0;\r\nmargin-bottom:	0;\r\nmargin-left:	0;\r\nmargin-right:	0;\r\nwidth:	100%;\r\n " + "}" + "\r\n";
                    originaltext = originaltext.Replace(temp, temp_new);
                    temp = ""; temp_new = "";

                    File.WriteAllText(path, originaltext);
                    MessageBox.Show("Изменения приняты");
                }
                else
                    MessageBox.Show("Файл не найден");
            }
        }


        private void button4_Click(object sender, EventArgs e)
        {
            ParseCSSPresentation();
            if (proverka == true)
            {
                SaveOtkatStyle();
                otkats.Clear();
                if (!Directory.Exists("C:\\Projects\\CSS\\"))
                    Directory.CreateDirectory("C:\\Projects\\CSS\\");
                if (!Directory.Exists("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\"))
                    Directory.CreateDirectory("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\");
                method(new DirectoryInfo("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\"));
                i = otkats.Count() - 1;
                if (i == (otkats.Count() - 1))
                    button25.Enabled = false;
                else button25.Enabled = true;
                if (i > 0)
                    button24.Enabled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tablerow2td_txtbox_border3.Text = ChoiseOfColor();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            header_txtbox_textcolor.Text = ChoiseOfColor();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            tablerow1td_txtbox_shriftcolor.Text =  ChoiseOfColor();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            h1_txtbox_textcolor.Text = ChoiseOfColor();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ParseCSSPresentation();
            if (proverka == true)
            {
                SaveOtkatStyle();
                otkats.Clear();
                if (!Directory.Exists("C:\\Projects\\CSS\\"))
                    Directory.CreateDirectory("C:\\Projects\\CSS\\");
                if (!Directory.Exists("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\"))
                    Directory.CreateDirectory("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\");
                method(new DirectoryInfo("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\"));
                i = otkats.Count() - 1;
                if (i == (otkats.Count() - 1))
                    button25.Enabled = false;
                else button25.Enabled = true;
                if (i > 0)
                    button24.Enabled = true;
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            body2_txtbox_color.Text = ChoiseOfColor();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            body2_txtbox_borderbottom3.Text = ChoiseOfColor();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            body2_txtbox_borderleft3.Text = ChoiseOfColor();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            bodytd_txtbox_color.Text = ChoiseOfColor();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            bodytd_txtbox_border3.Text = ChoiseOfColor();
            
        }

        private void button16_Click(object sender, EventArgs e)
        {
            bodytd_txtbox_borderleft3.Text = ChoiseOfColor();
        }

        public struct TempCSS
        {
            public string boxname;
            public string value;
        }

        List<TempCSS> tempor = new List<TempCSS>();
        List<TempCSS> tempor2 = new List<TempCSS>();

        private void CreateXMLDocument(string filepath)
        {
            XmlTextWriter xtw = new XmlTextWriter(filepath, Encoding.UTF8);
            xtw.WriteStartDocument();
            xtw.WriteStartElement("Project");
            xtw.WriteEndDocument();
            xtw.Close();
        }


        public void savestyle()
        {
            NameOFCSSStyle10 form = new NameOFCSSStyle10();
            form.ShowDialog();
            if (form.OkCan == "OK")
            {
                string nameofcss = form.style();
                TempCSS temp = new TempCSS();
                List<Control> availControls = GetControls(this);
                foreach (Control tmp in availControls)
                {
                    if (tmp.GetType().ToString() == "System.Windows.Forms.TextBox" || tmp.GetType().ToString() == "System.Windows.Forms.ComboBox" || tmp.GetType().ToString() == "System.Windows.Forms.NumericUpDown")//System.Windows.Forms.TextBox  System.Windows.Forms.ComboBox  System.Windows.Forms.NumericUpDown
                    {
                        temp.boxname = tmp.Name;
                        temp.value = tmp.Text;
                        tempor.Add(temp);
                    }
                }
                if (!Directory.Exists("C:\\Projects\\CSS\\Universal_Styles\\"))
                    Directory.CreateDirectory("C:\\Projects\\CSS\\Universal_Styles\\");
                if (!File.Exists("C:\\Projects\\CSS\\Universal_Styles\\" + nameofcss + ".xml"))
                    CreateXMLDocument("C:\\Projects\\CSS\\Universal_Styles\\" + nameofcss + ".xml");
                else if (File.Exists("C:\\Xml\\CSS\\Universal_Styles\\" + nameofcss + ".xml"))
                {
                    File.Delete("C:\\Projects\\CSS\\Universal_Styles\\" + nameofcss + ".xml");
                    CreateXMLDocument("C:\\Projects\\CSS\\Universal_Styles\\" + nameofcss + ".xml");
                }

                string type = "";
                int lineInt = 0;
                string source = "C:\\Projects\\CSS\\Universal_Styles\\" + nameofcss + ".xml";
                int.TryParse("CSS", out lineInt);

                // Cоздаем экземпляр класса
                XmlDocument Document = new XmlDocument();

                // Загружаем XML файл
                Document.Load(source);

                XmlNode root = Document.DocumentElement;

                //  для List<T> XmlNode t = Document.SelectNodes();

                XmlElement entryElement = Document.CreateElement("entry");
                entryElement.SetAttribute("type", type);

                foreach (var t in tempor)
                {
                    XmlNode node2 = Document.CreateNode(XmlNodeType.Element, "Element", "");
                    root.AppendChild(node2);

                    XmlElement nod7 = Document.CreateElement("NameOfTextBox");
                    nod7.InnerText = t.boxname;
                    node2.AppendChild(nod7);

                    XmlElement nod8 = Document.CreateElement("ValueOfAttribute");
                    nod8.InnerText = t.value;
                    node2.AppendChild(nod8);
                }

                Document.Save(source);
                tempor.Clear();
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            savestyle();
        }

        public static List<Control> GetControls(Control form)
        {
            var controlList = new List<Control>();

            foreach (Control childControl in form.Controls)
            {
                // Recurse child controls.
                controlList.AddRange(GetControls(childControl));
                controlList.Add(childControl);
            }
            return controlList;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            ChooseCSSStyle9 form = new ChooseCSSStyle9();
            form.ShowDialog();
            string style = form.style();
            if (form.OkCan == "OK" && style != null)
            {
                form.Close();

                TempCSS temp = new TempCSS();
                // Объявляем и забиваем файл в документ   
                XmlDocument xd = new XmlDocument();
                FileStream fs = new FileStream("C:\\Projects\\CSS\\Universal_Styles\\" + style + ".xml", FileMode.Open);
                xd.Load(fs);
                XmlNodeList nodes = xd.ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    if (node.Name == "Project")
                        foreach (XmlNode node2 in node.ChildNodes)
                        {
                            foreach (XmlElement attr in node2.ChildNodes)
                            {
                                switch (attr.Name)
                                {
                                    case "NameOfTextBox":
                                        {
                                            temp.boxname = attr.InnerText;
                                            break;
                                        }
                                    case "ValueOfAttribute":
                                        {
                                            temp.value = attr.InnerText;
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                            tempor2.Add(temp);
                        }
                }
                // Закрываем поток   
                fs.Close();

                string otkatname = string.Format("{0:yyyy-MM-dd_hh_mm_ss}", DateTime.Now);
                File.Copy("C:\\Projects\\CSS\\Universal_Styles\\" + style + ".xml", "C:\\Projects\\CSS\\CSS_" + project.ProjectName + "\\" + otkatname + ".xml");
                otkats.Clear();
                method(new DirectoryInfo("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\"));
                i = otkats.Count() - 1;
                if (i == 0)
                    button24.Enabled = false;
                else
                    button24.Enabled = true;

            }

            List<Control> availControls = GetControls(this);
            foreach (Control tmp in availControls)
            {
                if (tmp.GetType().ToString() == "System.Windows.Forms.TextBox" || tmp.GetType().ToString() == "System.Windows.Forms.ComboBox" || tmp.GetType().ToString() == "System.Windows.Forms.NumericUpDown")//System.Windows.Forms.TextBox  || System.Windows.Forms.ComboBox
                {
                    foreach (var t in tempor2)
                    {
                        if (tmp.Name == t.boxname)
                            tmp.Text = t.value;
                    }
                }
            }

        }

        List<string> otkats = new List<string>();

        private void method(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();
            foreach (var t in files)
            {
                otkats.Add(t.FullName);
            }

        }

        int i = 0;

        private void method_Next()
        {            
            i++;   
            TempCSS temp = new TempCSS();
            // Объявляем и забиваем файл в документ   
            XmlDocument xd = new XmlDocument();
            FileStream fs = new FileStream(otkats.ElementAt(i), FileMode.Open);
            xd.Load(fs);
            XmlNodeList nodes = xd.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "Project")
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        foreach (XmlElement attr in node2.ChildNodes)
                        {
                            switch (attr.Name)
                            {
                                case "NameOfTextBox":
                                    {
                                        temp.boxname = attr.InnerText;
                                        break;
                                    }
                                case "ValueOfAttribute":
                                    {
                                        temp.value = attr.InnerText;
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                        tempor2.Add(temp);
                    }
            }
            fs.Close();

            List<Control> availControls = GetControls(this);
            foreach (Control tmp in availControls)
            {
                if (tmp.GetType().ToString() == "System.Windows.Forms.TextBox" || tmp.GetType().ToString() == "System.Windows.Forms.ComboBox" || tmp.GetType().ToString() == "System.Windows.Forms.NumericUpDown")//System.Windows.Forms.TextBox
                {
                    foreach (var t in tempor2)
                    {
                        if (tmp.Name == t.boxname)
                            tmp.Text = t.value;
                    }
                }
            }
            if (i == (otkats.Count()-1))
                button25.Enabled = false;
            else button25.Enabled = true;
            if (i > 0)
                button24.Enabled = true;

        }

        private void method_Prev()
        {
                i--;
                TempCSS temp = new TempCSS();
                // Объявляем и забиваем файл в документ   
                XmlDocument xd = new XmlDocument();
                FileStream fs = new FileStream(otkats.ElementAt(i), FileMode.Open);
                xd.Load(fs);
                XmlNodeList nodes = xd.ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    if (node.Name == "Project")
                        foreach (XmlNode node2 in node.ChildNodes)
                        {
                            foreach (XmlElement attr in node2.ChildNodes)
                            {
                                switch (attr.Name)
                                {
                                    case "NameOfTextBox":
                                        {
                                            temp.boxname = attr.InnerText;
                                            break;
                                        }
                                    case "ValueOfAttribute":
                                        {
                                            temp.value = attr.InnerText;
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                            tempor2.Add(temp);
                        }
                }
                fs.Close();

                List<Control> availControls = GetControls(this);
                foreach (Control tmp in availControls)
                {
                    if (tmp.GetType().ToString() == "System.Windows.Forms.TextBox" || tmp.GetType().ToString() == "System.Windows.Forms.ComboBox" || tmp.GetType().ToString() == "System.Windows.Forms.NumericUpDown")//System.Windows.Forms.TextBox
                    {
                        foreach (var t in tempor2)
                        {
                            if (tmp.Name == t.boxname)
                                tmp.Text = t.value;
                        }
                    }
                }

                if (i == 0)
                    button24.Enabled = false;
                else
                    button24.Enabled = true;
                if (i < (otkats.Count() - 1))
                    button25.Enabled = true;
            
          
        }

        private void button28_Click(object sender, EventArgs e)
        {
            bodytext_txt_colortext.Text = ChoiseOfColor();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            bodytext_txt_fonecolor.Text = ChoiseOfColor();
        }

        // сохранение откатов в xml
        public void SaveOtkatStyle()
        {
            string otkatname = string.Format("{0:yyyy-MM-dd_hh_mm_ss}", DateTime.Now);
                string projectname = project.ProjectName;
                TempCSS temp = new TempCSS();
                List<Control> availControls = GetControls(this);
                foreach (Control tmp in availControls)
                {
                    if (tmp.GetType().ToString() == "System.Windows.Forms.TextBox" || tmp.GetType().ToString() == "System.Windows.Forms.ComboBox" || tmp.GetType().ToString() == "System.Windows.Forms.NumericUpDown")//System.Windows.Forms.TextBox
                    {
                        temp.boxname = tmp.Name;
                        temp.value = tmp.Text;
                        tempor.Add(temp);
                    }
                }
                if (!Directory.Exists("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS"))
                    Directory.CreateDirectory("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS");
                if (!File.Exists("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\" + otkatname + ".xml"))
                    CreateXMLDocument("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\" + otkatname + ".xml");
                else if (File.Exists("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\" + otkatname + ".xml"))
                {
                    File.Delete("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\" + otkatname + ".xml");
                    CreateXMLDocument("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\" + otkatname + ".xml");
                }

                string type = "";
                int lineInt = 0;
                string source = "C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\" + otkatname + ".xml";
                int.TryParse("CSS", out lineInt);

                // Cоздаем экземпляр класса
                XmlDocument Document = new XmlDocument();

                // Загружаем XML файл
                Document.Load(source);

                XmlNode root = Document.DocumentElement;

                //  для List<T> XmlNode t = Document.SelectNodes();

                XmlElement entryElement = Document.CreateElement("entry");
                entryElement.SetAttribute("type", type);

                foreach (var t in tempor)
                {
                    XmlNode node2 = Document.CreateNode(XmlNodeType.Element, "Element", "");
                    root.AppendChild(node2);

                    XmlElement nod7 = Document.CreateElement("NameOfTextBox");
                    nod7.InnerText = t.boxname;
                    node2.AppendChild(nod7);

                    XmlElement nod8 = Document.CreateElement("ValueOfAttribute");
                    nod8.InnerText = t.value;
                    node2.AppendChild(nod8);
                }

                Document.Save(source);
                tempor.Clear();
            }

        private void button20_Click(object sender, EventArgs e)
        {
            ParseCSSPresentation();
            if (proverka == true)
            {
                SaveOtkatStyle();
                otkats.Clear();
                if (!Directory.Exists("C:\\Projects\\CSS\\"))
                    Directory.CreateDirectory("C:\\Projects\\CSS\\");
                if (!Directory.Exists("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\"))
                    Directory.CreateDirectory("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\");
                method(new DirectoryInfo("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\"));
                i = otkats.Count() - 1;
                if (i == (otkats.Count() - 1))
                    button25.Enabled = false;
                else button25.Enabled = true;
                if (i > 0)
                    button24.Enabled = true;
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            foreach (string path in Directory.GetFiles("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\"))
            {                
                File.Delete(path);
            }
           otkats.Clear();
           i = 0;
           if (i == 0)
               button24.Enabled = false;
           else
               button24.Enabled = true;
           if (otkats.Count < 2)
               button25.Enabled = false;
           else button25.Enabled = true;
        }


        private void ChangeCSS_Load(object sender, EventArgs e)
        {
            SaveOtkatStyle();
            otkats.Clear();
            i = 0;
            method(new DirectoryInfo("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\"));
            if (i == 0)
                button24.Enabled = false;
            else
                button24.Enabled = true;
            if (otkats.Count < 2)
                button25.Enabled = false;
            else button25.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {

                TempCSS temp = new TempCSS();
                // Объявляем и забиваем файл в документ   
                XmlDocument xd = new XmlDocument();
                if (File.Exists("C:\\Projects\\CSS\\Universal_Styles\\" + "Original.xml"))
                {
                    FileStream fs = new FileStream("C:\\Projects\\CSS\\Universal_Styles\\" + "Original.xml", FileMode.Open);

                    xd.Load(fs);
                    XmlNodeList nodes = xd.ChildNodes;
                    foreach (XmlNode node in nodes)
                    {
                        if (node.Name == "Project")
                            foreach (XmlNode node2 in node.ChildNodes)
                            {
                                foreach (XmlElement attr in node2.ChildNodes)
                                {
                                    switch (attr.Name)
                                    {
                                        case "NameOfTextBox":
                                            {
                                                temp.boxname = attr.InnerText;
                                                break;
                                            }
                                        case "ValueOfAttribute":
                                            {
                                                temp.value = attr.InnerText;
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                }
                                tempor2.Add(temp);
                            }
                    }
                    // Закрываем поток   
                    fs.Close();


                    string otkatname = string.Format("{0:yyyy-MM-dd_hh_mm_ss}", DateTime.Now);
                    File.Copy("C:\\Projects\\CSS\\Universal_Styles\\" + "Original.xml", "C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\" + otkatname + ".xml");
                    otkats.Clear();
                    method(new DirectoryInfo("C:\\Projects\\CSS\\" + project.ProjectName + "_CSS\\"));
                    i = otkats.Count() - 1;
                    if (i == 0)
                        button24.Enabled = false;
                    else
                        button24.Enabled = true;

                    List<Control> availControls = GetControls(this);
                    foreach (Control tmp in availControls)
                    {
                        if (tmp.GetType().ToString() == "System.Windows.Forms.TextBox" || tmp.GetType().ToString() == "System.Windows.Forms.ComboBox" || tmp.GetType().ToString() == "System.Windows.Forms.NumericUpDown")//System.Windows.Forms.TextBox
                        {
                            foreach (var t in tempor2)
                            {
                                if (tmp.Name == t.boxname)
                                    tmp.Text = t.value;
                            }
                        }
                    }
                }
                else
                    MessageBox.Show("Пожалуйста, создайте файл стиля с названием оригинал!");
             
        }

        private void button25_Click_1(object sender, EventArgs e)
        {
            method_Next();
        }

        private void button24_Click_1(object sender, EventArgs e)
        {
            method_Prev();
        }

    }
}
