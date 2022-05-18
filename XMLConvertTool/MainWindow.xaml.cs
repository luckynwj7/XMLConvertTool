using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace XMLConvertTool
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow mainWinObj;
        public static MainWindow MainWinObj
        {
            get
            {
                if (mainWinObj is null)
                {
                    mainWinObj = new MainWindow();
                    return mainWinObj;
                }
                else
                {
                    return mainWinObj;
                }
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void checkFileFindButton_Click(object sender, RoutedEventArgs e)
        {
            FileDialogManager.ShowFolderOpenDialog(checkFileTextBox);
        }

        private void saveFolderButton_Click(object sender, RoutedEventArgs e)
        {
            FileDialogManager.ShowFolderOpenDialog(saveFolderTextBox);
        }


        private void convertStartBtn_Click(object sender, RoutedEventArgs e)
        {
            string loadPath = checkFileTextBox.Text;
            string savePath = saveFolderTextBox.Text;
            if (loadPath == "")
            {
                MessageBox.Show("불러올 폴더 경로가 입력되지 않음");
                return;
            } else if (savePath == "") {
                MessageBox.Show("저장할 폴더 경로가 입력되지 않음");
                return;
            }

            string[] fileNames = GetSearchSVGFile(loadPath);
            foreach (string fileName in fileNames)
            {
                string contentsStr = System.IO.File.ReadAllText(fileName);
                string result = TextAllConvert(contentsStr);
                string newFileName = savePath + FileNameChange(fileName, saveNewFileNameTextBox.Text, loadPath);

                int endIndex = FindCharIndexInStr(newFileName.Length -1, newFileName, '\\', true);
                string newFilePath = newFileName.Substring(0, endIndex);
                DirectoryInfo di = new DirectoryInfo(newFilePath);
                if (di.Exists == false)
                {
                    di.Create();
                }
                System.IO.File.WriteAllText(newFileName, result, Encoding.UTF8);
            }
            checkFileTextBox.Text = saveFolderTextBox.Text;
            saveNewFileNameTextBox.Text = "";
            MessageBox.Show("변환 완료");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void eixtAppBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        ///////////////////
        public string[] GetSearchSVGFile(string path)
        {
            string[] files = { "", };
            try { 
                files = Directory.GetFiles(path, "*.svg", SearchOption.AllDirectories);
            } catch (IOException ex) { 
                MessageBox.Show(ex.Message); 
            }
            return files;
        }

        public string TextAllConvert(string text)
        {
            for (int i = 0; i < text.Length; ++i)
            {
                if (text[i] == '<')
                {
                    int endIndex = FindCharIndexInStr(i, text, '>', false);
                    if (endIndex >= text.Length - 1)
                    {
                        break;
                    }
                    string rowStr = text.Substring(i, endIndex - i + 1);
                    if ((bool)opacityApplyCheckBox.IsChecked)
                    {
                        rowStr = RowOpacityConvert(rowStr);
                    }
                    rowStr = RowAtrributeValueConvert(rowStr);
                    
                    text = text.Remove(i, endIndex - i + 1);
                    text = text.Insert(i, rowStr);

                    i += rowStr.Length;
                }
            }
            return text;
        }

        public string RowOpacityConvert(string rowStr)
        {
            string fillOpacityStr = " fill-opacity=\"";
            if (rowStr.Length <= fillOpacityStr.Length)
            {
                return rowStr;
            }

            // 태그 내에서 fill-opacity의 속성 뽑아내기
            int fillOpacityStartIndex = rowStr.IndexOf(fillOpacityStr) + fillOpacityStr.Length;
            int fillOpacityEndIndex = FindCharIndexInStr(fillOpacityStartIndex + 1, rowStr, '\"', false);
            string value = rowStr.Substring(fillOpacityStartIndex, fillOpacityEndIndex - fillOpacityStartIndex);


            if (rowStr.Contains(fillOpacityStr) && !rowStr.Contains(" fill=\""))
            {
                // 이미 opacity 속성이 있는지 검사
                string convertStr = " fill=\"#000000\"" + fillOpacityStr + value + "\"";
                rowStr = rowStr.Replace(fillOpacityStr + value + "\"", convertStr);
            }
            return rowStr;
        }

        public string RowAtrributeValueConvert(string rowStr)
        {
            string attrStr = " " + attrNameTextBox.Text + "=\"";
            if (rowStr.Length <= attrStr.Length || !rowStr.Contains(attrStr))
            {
                return rowStr;
            }

            int attrStartIndex = rowStr.IndexOf(attrStr) + attrStr.Length;
            int attrEndIndex = FindCharIndexInStr(attrStartIndex + 1, rowStr, '\"', false);
            string value = rowStr.Substring(attrStartIndex, attrEndIndex - attrStartIndex);
            if (value != attrValueOriginTextBox.Text)
            {
                return rowStr;
            }

            rowStr = rowStr.Remove(attrStartIndex, attrEndIndex - attrStartIndex);
            rowStr = rowStr.Insert(attrStartIndex, attrValueNewTextBox.Text);
            return rowStr;
        }


        public int FindCharIndexInStr(int currentIndex, string text, char c, bool isPrev)
        {
            if (isPrev) {
                for (int i = currentIndex; i >= 0; i--)
                {
                    if (text[i] == c)
                    {
                        return i;
                    }
                }
            } else {
                for (int i = currentIndex; i < text.Length; i++)
                {
                    if (text[i] == c)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public string FileNameChange(string text, string addText, string rootDir)
        {
            int startIndex = rootDir.Length;
            int endIndex = text.Length - 4;
            string fileRealName = text.Substring(startIndex, endIndex - startIndex);
            fileRealName += addText;
            fileRealName += ".svg";

            return fileRealName;
        }
    }
}