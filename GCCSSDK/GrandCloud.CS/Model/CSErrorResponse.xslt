<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" exclude-result-prefixes="xsl">
	<xsl:output method="xml" omit-xml-declaration="no" indent="yes"/>

	<xsl:template match="Error">
    <xsl:element name="CSError">
      <Code>
        <xsl:value-of select="Code"/>
      </Code>
      <Message>
        <xsl:value-of select="Message"/>
      </Message>
      <RequestId>
        <xsl:value-of select="RequestId"/>
      </RequestId>
      <ETag>
        <xsl:value-of select="ETag"/>
      </ETag>
      <Resource>
        <xsl:value-of select="Resource"/>
      </Resource>
    </xsl:element>
	</xsl:template>
</xsl:stylesheet>