namespace EventServer.Core.Domain
{
    public class SpeakerAccolade : Entity
    {
        // The idea is to use this for things like the following
        // ASPInsider
        // Microsoft Reginal Director
        // INETA Speaker
        // Microsoft Certified Trainer (MCT)
        // Microsoft Certified Professional (MCP)
        //
        // Not sure this will be of interest, putting it here
        // because it is on DevEvents.com and this will force us
        // to talk about it.
        public string Title { get; set; }
    }
}