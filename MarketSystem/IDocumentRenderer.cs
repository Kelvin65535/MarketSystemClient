using System.Windows.Documents;

namespace MarketSystem
{
    public interface IDocumentRenderer
    {
        void Render(FlowDocument doc, object data);
    }
}