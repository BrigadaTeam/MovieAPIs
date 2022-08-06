namespace MovieAPIs.Models
{ 

    public enum Tops
    {
        TOP_250_BEST_FILMS,
        TOP_100_POPULAR_FILMS,
        TOP_AWAIT_FILMS
    }

    public enum MovieOrder
    {
        RATING,
        NUM_VOTE,
        YEAR
    }

    public enum MovieType
    {
        FILM,
        TV_SHOW,
        TV_SERIES,
        MINI_SERIES,
        ALL
    }

    public enum Filter
    {
        ALL = -1
    }


    public enum Months
    {
        JANUARY, 
        FEBRUARY, 
        MARCH, 
        APRIL, 
        MAY, 
        JUNE, 
        JULY, 
        AUGUST, 
        SEPTEMBER, 
        OCTOBER, 
        NOVEMBER, 
        DECEMBER
    }

    public enum ReviewOrder
    {
        DATE_ASC, 
        DATE_DESC, 
        USER_POSITIVE_RATING_ASC, 
        USER_POSITIVE_RATING_DESC,
        USER_NEGATIVE_RATING_ASC, 
        USER_NEGATIVE_RATING_DESC
    }

    public enum ImageType
    {
        STILL, 
        SHOOTING, 
        POSTER, 
        FAN_ART, 
        PROMO, 
        CONCEPT, 
        WALLPAPER, 
        COVER, 
        SCREENSHOT
    }
}