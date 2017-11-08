<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" encoding="utf-8" indent="yes" omit-xml-declaration="no"/>
  <xsl:template match="/">
    <CDCatalog>
      <Header>
        <CollectionInfo>TestValue</CollectionInfo>
      </Header>
          <xsl:for-each select="catalog/cd">
            <CDRecord>
              <Title>
                <xsl:value-of select="title"/>
              </Title>
              <Artist>
                <xsl:value-of select="artist"/>
              </Artist>
            </CDRecord>
          </xsl:for-each>
    </CDCatalog>
  </xsl:template>
</xsl:stylesheet>
