<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="xml" encoding="UTF-8" indent="yes" omit-xml-declaration="no"/>
  <xsl:param name="date">2015-01-01+12:00</xsl:param>
  <xsl:param name="time">16:09:24.123+11:00</xsl:param>
  <xsl:variable name="dateTime" select="concat($date,'T',$time)"/>
  <xsl:template match="/">
    <Date>
      <xsl:value-of select="format-date($date, '[M01]/[D01]/[Y0001]')" />
    </Date>
  </xsl:template>
</xsl:stylesheet>

