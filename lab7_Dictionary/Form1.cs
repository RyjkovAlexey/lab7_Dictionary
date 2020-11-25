using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab7_Dictionary
{
    enum Operation
    {
        merge,
        crossing,
        subtraction
    }
    public partial class Form1 : Form
    {
        List<MyDictionary> myDictionaries = new List<MyDictionary>();
        MyDictionary currentDictionary = new MyDictionary("Default");
        private Operation selectedOperation;
        BinaryFormatter formatter = new BinaryFormatter();
        public Form1()
        {
            InitializeComponent();
        }

        private void RefreshDictionarys()
        {
            listBox1.Items.Clear();
            lbFirstDict.Items.Clear();
            lbTwoDict.Items.Clear();
            myDictionaries.ForEach(dictionary =>
            {
                listBox1.Items.Add(dictionary.NameDictionary);
                lbFirstDict.Items.Add(dictionary.NameDictionary);
                lbTwoDict.Items.Add(dictionary.NameDictionary);
            });
        }

        private void RefreshWords()
        {
            if (currentDictionary != null)
            {
                listBox2.Items.Clear();
                foreach (var keyValuePair in currentDictionary.Dictionary)
                {
                    listBox2.Items.Add($"{keyValuePair.Key}\t-\t{keyValuePair.Value}");
                }
            }
        }

        private bool CheckName(string name)
        {
            foreach (var VARIABLE in myDictionaries)
            {
                if (VARIABLE.NameDictionary.Equals(name))
                {
                    return false;
                }
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length>=3&&CheckName(textBox1.Text))
            {
                MyDictionary newDict = new MyDictionary(textBox1.Text);
                currentDictionary = newDict;
                myDictionaries.Add(newDict);
                RefreshDictionarys();
                RefreshWords();
            }
            else
            {
                MessageBox.Show("Имя нового словаря должно быть не короче трех символов, либо такой словарь существует");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentDictionary = myDictionaries[listBox1.SelectedIndex];
            RefreshWords();
        }

        private void btnAddWord_Click(object sender, EventArgs e)
        {
            if (tbTranslationWord.Text.Length>0&&tbAddedWord.Text.Length>0)
            {
                currentDictionary.Add(tbAddedWord.Text, tbTranslationWord.Text);
                RefreshWords();
            }
            else
            {
                MessageBox.Show("Не заполнено одно из полей");
            }
        }

        private void tbTranslatableWord_TextChanged(object sender, EventArgs e)
        {
            lbTranslate.Text = currentDictionary.Search(tbTranslatableWord.Text);
        }

        private void rbMergin_CheckedChanged(object sender, EventArgs e)
        {
            selectedOperation = Operation.merge;
        }

        private void rbCrossing_CheckedChanged(object sender, EventArgs e)
        {
            selectedOperation = Operation.crossing;
        }

        private void rbSubtraction_CheckedChanged(object sender, EventArgs e)
        {
            selectedOperation = Operation.subtraction;
        }

        private void btnOperStart_Click(object sender, EventArgs e)
        {
            if (tbNewDictionary.Text.Length>=3&&lbFirstDict.SelectedIndex!=lbTwoDict.SelectedIndex&&CheckName(tbNewDictionary.Text))
            {
                MyDictionary firstDict = myDictionaries[lbFirstDict.SelectedIndex];
                MyDictionary twoDict = myDictionaries[lbTwoDict.SelectedIndex];
                switch (selectedOperation)
                {
                    case Operation.crossing:
                    {
                        MyDictionary newDict = firstDict.Crossing(twoDict, tbNewDictionary.Text);
                        currentDictionary = newDict;
                        myDictionaries.Add(newDict);
                        RefreshDictionarys();
                        RefreshWords();
                        break;
                    }
                    case Operation.merge:
                    {
                        MyDictionary newDict = firstDict.Merge(twoDict, tbNewDictionary.Text);
                        currentDictionary = newDict;
                        myDictionaries.Add(newDict);
                        RefreshDictionarys();
                        RefreshWords();
                        break;
                    }
                    case Operation.subtraction:
                    {
                        MyDictionary newDict = firstDict.Subtraction(twoDict, tbNewDictionary.Text);
                        currentDictionary = newDict;
                        myDictionaries.Add(newDict);
                        RefreshDictionarys();
                        RefreshWords();
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Не выбран один из двух словарей, либо имя нового словаря менее 3-х символов или же словарь с таким именем существует");
            }
        }

        private void btnDeleteWord_Click(object sender, EventArgs e)
        {
            string selectedWord = listBox2.SelectedItem.ToString().Split('\t')[0];
            currentDictionary.Remove(selectedWord);
            RefreshWords();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (FileStream fs = new FileStream("dictionary.dat", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, myDictionaries);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (File.Exists("dictionary.dat"))
            {
                using (FileStream fs = new FileStream("dictionary.dat", FileMode.OpenOrCreate))
                {
                    var list = (List<MyDictionary>)formatter.Deserialize(fs);
                    myDictionaries = list;
                    currentDictionary = myDictionaries.First();
                }
            }
            else
            {
                myDictionaries.Add(currentDictionary);
                currentDictionary.Add("Тест", "Test");
            }
            RefreshDictionarys();
            RefreshWords();
        }
    }
}
