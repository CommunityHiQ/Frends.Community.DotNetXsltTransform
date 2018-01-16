<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="no"/>
  <xsl:param name="testParameter"/>
  <xsl:template match="/">
    <ValueOfParameter>
      <xsl:value-of select="$testParameter" />
    </ValueOfParameter>
  </xsl:template>
</xsl:stylesheet>

