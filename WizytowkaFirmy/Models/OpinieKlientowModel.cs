using System.ComponentModel.DataAnnotations;
using WizytowkaFirmy.Models.DbModels;

namespace WizytowkaFirmy.Models
{
    public class OpinieKlientowModel
    {
        public List<OpiniaKlienta> ListaOpinii { get; set; } = new List<OpiniaKlienta>();
        public OpiniaKlienta NowaOpinia { get; set; } = new OpiniaKlienta
        {
            Klient = new Klient()
        };
    }
}
