using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.Core.Extension;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SHSecurityModels;

namespace KVDDDCore.Utils
{
    public delegate void ReadRowCells(List<ICell> rowCells);

    public class NPOIHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">文件名</param>
        /// <param name="sheetIndex">sheetIndex</param>
        /// <param name="RowCellCount">单行共多少格</param>
        /// <param name="rowCells"></param>
        public static void ReadXlsx(string filePath, int sheetIndex, ReadRowCells rowCells)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);

                ISheet sheet1 = workbook.GetSheetAt(sheetIndex);

                int rowNum = sheet1.LastRowNum;
                int colNum = sheet1.GetRow(0).LastCellNum;

                for (int i = 1; i < rowNum; i++)
                {
                    IRow row = sheet1.GetRow(i);
                    //var Cels = row.Cells;
                    List<ICell> list = new List<ICell>();
                    for (int m = 0; m < colNum; m++)
                    {
                        ICell cell = row.GetCell(m);
                        list.Add(cell);
                    }

                    //var CelsCount = Cels.Count;
                    //if (CelsCount < RowCellCount)
                    //{
                    //    for (int m = 0; m < RowCellCount - CelsCount; m++)
                    //    {
                    //        Cels.Add(null);
                    //    }
                    //}

                    if (rowCells != null)
                    {
                        rowCells(list);
                    }
                }
                workbook.Close();
                //sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 10));
                //var rowIndex = 0;
                //IRow row = sheet1.CreateRow(rowIndex);
                //row.Height = 30 * 80;
                //row.CreateCell(0).SetCellValue("this is content");
                //sheet1.AutoSizeColumn(0);
                //rowIndex++;

                //var sheet2 = workbook.CreateSheet("Sheet2");
                //var style1 = workbook.CreateCellStyle();
                //style1.FillForegroundColor = HSSFColor.Blue.Index2;
                //style1.FillPattern = FillPattern.SolidForeground;

                //var style2 = workbook.CreateCellStyle();
                //style2.FillForegroundColor = HSSFColor.Yellow.Index2;
                //style2.FillPattern = FillPattern.SolidForeground;

                //var cell2 = sheet2.CreateRow(0).CreateCell(0);
                //cell2.CellStyle = style1;
                //cell2.SetCellValue(0);

                //cell2 = sheet2.CreateRow(1).CreateCell(0);
                //cell2.CellStyle = style2;
                //cell2.SetCellValue(1);

                //workbook.Write(fs);
            }
        }



        public static Task<bool> WriteXlsx(string path, IQueryable<MQServerData> list)
        {
            return Task.Run(() =>
            {
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet sheet1 = workbook.CreateSheet("1");

                    int num = 1;
                    IRow row0 = sheet1.CreateRow(0);
                    row0.CreateCell(0).SetCellValue("commRmk");
                    row0.CreateCell(1).SetCellValue("dsnum");
                    row0.CreateCell(2).SetCellValue("foreignld");
                    row0.CreateCell(3).SetCellValue("mtype");
                    row0.CreateCell(4).SetCellValue("projectId");
                    row0.CreateCell(5).SetCellValue("time");
                    row0.CreateCell(6).SetCellValue("userId");
                    row0.CreateCell(7).SetCellValue("topicType");
                    row0.CreateCell(8).SetCellValue("timeStamp");
                    foreach (var item in list)
                    {
                        IRow row = sheet1.CreateRow(num++);
                        row.CreateCell(0).SetCellValue(item.commRmk);
                        row.CreateCell(1).SetCellValue(item.dsnum);
                        row.CreateCell(2).SetCellValue(item.foreignld);
                        row.CreateCell(3).SetCellValue(item.mtype);
                        row.CreateCell(4).SetCellValue(item.projectId);
                        row.CreateCell(5).SetCellValue(item.time);
                        row.CreateCell(6).SetCellValue(item.userId);
                        row.CreateCell(7).SetCellValue(item.topicType);
                        row.CreateCell(8).SetCellValue(item.timeStamp);
                    }
                    workbook.Write(fs);
                    workbook.Close();
                }
                return true;
            });
        
        }
    }
}
