# rsstowp

Save your rss feed to a Wordpress blog.

Instructions:
1. Create new blog, and use this exclusively for your feed.
2. Change posts per page to max.
3. Create the Keys.xml per spec with your data.
4. Install https://wordpress.org/plugins/image-teleporter/ plugin for flavor to save images locally. (In case the external links disappear)
5. Launch the program.


As it stands atm put Keys.xml in your app launch directory.

```xml
<UserData>
  <ReaderAPI>
    <Token>123456...</Token>
  </ReaderAPI>
	<Feed>
		<Url>http://feeds.example.com/examples</Url>
	</Feed>
	<WordPress>
		<Site>http://www.example.com/wp</Site>
		<Username>user</Username>
		<Password>12345</Password>
	</WordPress>
</UserData>
```

Uses http://abrudtkuhl.github.io/WordPressSharp/ .
