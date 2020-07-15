using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiltrForm
{
    class TopHeaderClass
    {
        public string filename;
        public string extension;
        public string error;
        public DataTable dt;
        public DataTable dt2;
        private IExcelDataReader edr;

        public int Id { get; set; }
        public string Name { get; set; }
        public string StrtName { get; set; }
        public string Symbol { get; set; }



        public TopHeaderClass(string file)
        {
            this.filename = file;
            this.extension = filename.Substring(filename.LastIndexOf('.'));
            ReadContent();
            string[] columnNames = dt.Columns.Cast<DataColumn>()
                         .Select(x => x.ColumnName)
                         .ToArray();
            this.dt2 = CreateDt(columnNames);

        }

        private DataTable CreateDt(string[] columnNames)
        {
            dt2 = new DataTable();
            
            dt2.Columns.Add(new DataColumn("INT_ID_COLUMN", System.Type.GetType("System.Int32")));
            dt2.Columns.Add(new DataColumn("Active", System.Type.GetType("System.Boolean")));
            dt2.Columns.Add(new DataColumn("Name_Col", System.Type.GetType("System.String")));
            dt2.Columns.Add(new DataColumn("Psevdonim", System.Type.GetType("System.String")));
            dt2.Columns.Add(new DataColumn("Symbol", System.Type.GetType("System.String")));
            dt2.Columns.Add(new DataColumn("Znak", System.Type.GetType("System.String")));

            // теперь с таблице можно работать
            foreach (string colName in columnNames)
            {
                DataRow rowToAdd = dt2.NewRow();
                rowToAdd["INT_ID_COLUMN"] = 1;
                rowToAdd["Active"] = false;
                rowToAdd["Name_Col"] = colName;
                rowToAdd["Psevdonim"] = "";
                rowToAdd["Symbol"] = "=";
                rowToAdd["Znak"] = "and";
                dt2.Rows.Add(rowToAdd);
            }
            return dt2;
            

        }

        private void ReadContent()
        {
            try
            {

                // Создаем поток для чтения.
                var stream = File.Open(filename, FileMode.Open, FileAccess.Read);
                // В зависимости от расширения файла Excel, создаем тот или иной читатель.
                // Читатель для файлов с расширением *.xlsx.
                if (extension == ".xlsx")
                    edr = ExcelReaderFactory.CreateOpenXmlReader(stream);
                // Читатель для файлов с расширением *.xls.
                else if (extension == ".xls")
                    edr = ExcelReaderFactory.CreateBinaryReader(stream);

                //// reader.IsFirstRowAsColumnNames
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };
                // Читаем, получаем DataSet и работаем с ним как обычно.
                var dataSet = edr.AsDataSet(conf);
                dt = dataSet.Tables[0];

                // После завершения чтения освобождаем ресурсы.
                edr.Close();

                if (dt.Columns.Count < 9)
                    this.error = "Должно быть 9 столбцов";
            }
            catch (Exception ex)
            {
                this.error = extension + " " + ex.Message;
            }
        }


    }
}
