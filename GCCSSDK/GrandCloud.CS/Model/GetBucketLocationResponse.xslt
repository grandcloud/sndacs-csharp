<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  exclude-result-prefixes="xsl">
	<xsl:output method="xml" omit-xml-declaration="no" indent="yes"/>	
	<xsl:template match="LocationConstraint">
		<xsl:element name="GetBucketLocationResponse">
			<xsl:choose>
				<xsl:when test="../LocationConstraint=''">
					<xsl:element name="Location"></xsl:element>
				</xsl:when>
				<xsl:otherwise>
					<xsl:element name="Location">
						<xsl:value-of select="../LocationConstraint"/>
					</xsl:element>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>