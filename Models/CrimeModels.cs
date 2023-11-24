public record CrimeSchema(
    string caseId,
    DateTime crimeDate,
    string crimeDescription,
    float latitude,
    float longitude,
    string locationDescription,
    string iucrPrimaryDescription,
    string iucrSecondaryDescription,
    bool iucrActive,
    string caseNumber,
    string primaryType,
    bool arrest
) {}

public record CrimePageAPI(
    List<CrimeSchema> crime,
    int noOfpages
) {}