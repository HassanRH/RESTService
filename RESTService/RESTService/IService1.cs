using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RESTService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1 //de metoder der bliver lavet er det man kan se 
    {
        [OperationContract] // Oprationcontract definer alt det som vi har her nede.. den håndtere metoden
        [WebInvoke(Method = "GET", 
            ResponseFormat = WebMessageFormat.Json, // det er nemmer syns mæssigt med jason det er derfor vi convetere det til jason format..
            UriTemplate = "passager/")] //det man skriver i adressefeltet - (efter localhost) i browseren
        IList<Passager> GetAllpassager();

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "passager/navn/{navn}")] // der står navn for vi vil have et specifik navn i stedet for det hele
        IList<Passager> GetpassagerByNavn(string navn);

        [OperationContract]
        [WebInvoke(Method = "GET",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "passager/{Id}")]
        Passager GetPassagerById(string Id); //int skal conveteres til en string

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "passager/{id}")]
        void DeletePassager(string id); //man forventer ikke at få noget tilbage når man sletter noget der for har man void med 

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            UriTemplate = "passager/")]
        int PostPassager(Passager passger);

        [OperationContract]
        [WebInvoke(Method = "PUT",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "passager/{id}")]
        int UpdatePassager(string id, Passager passager);


    }
}
