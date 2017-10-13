# CompanyPlaylist

This will take a list of requests for songs to play.  There are two 30min sections and the requested song will only be played if the requester is available during that section

## Assumptions
* Only two periods (1 and 2)
* Everyone who requested a song, also have availability set
* Case matters
* On repeat songs, number of times played takes priority over requester
		Example: Lisa and Clint have songs. Last song played was Lisa's.  Clients have all been played twice, 
			the next song will be Lisa's again as hers have only been played once assuming the fit the time period.
