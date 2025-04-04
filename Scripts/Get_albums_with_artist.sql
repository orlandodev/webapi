-- Script Date: 3/23/2025 8:04 AM  - ErikEJ.SqlCeScripting version 3.5.2.95
SELECT a.[AlbumId]
      ,a.[Title]
      ,b.[Name]
  FROM [Album] a
  JOIN [Artist] b on a.ArtistId = b.ArtistId
  ORDER BY b.[Name];