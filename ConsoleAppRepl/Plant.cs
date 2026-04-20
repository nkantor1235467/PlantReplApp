namespace Domain;

//this file contains the parameters for a class Plant
class Plant
{
public string Genus {get;set;}

public string Species {get;set;}

public int HowMany {get;set;}

public Guid PlantID {get;} = Guid.NewGuid();

public string Display()
    {
        return $"ID: {PlantID} \nNumber of plants: {HowMany:C}";
    }

}