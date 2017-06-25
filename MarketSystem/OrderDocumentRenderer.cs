using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace MarketSystem
{
    class OrderDocumentRenderer : IDocumentRenderer
    {
        public void Render(FlowDocument doc, object data)
        {
            TableRowGroup group = doc.FindName("rowsDetails") as TableRowGroup;
            Style styleCell = doc.Resources["BorderedCell"] as Style;
            foreach (ShopItem item in ((OrderData)data).itemList)
            {
                TableRow row = new TableRow();

                TableCell cell = new TableCell(new Paragraph(new Run(item.ItemName)));
                cell.Style = styleCell;
                row.Cells.Add(cell);

                cell = new TableCell(new Paragraph(new Run(item.ItemCount.ToString())));
                cell.Style = styleCell;
                row.Cells.Add(cell);

                cell = new TableCell(new Paragraph(new Run((item.ItemCount * item.ItemSellPrice).ToString("C"))));
                cell.Style = styleCell;
                row.Cells.Add(cell);
                
                group.Rows.Add(row);
            }
        }
    }
}
