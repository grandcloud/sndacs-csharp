<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" exclude-result-prefixes="xsl">
  <xsl:output method="xml" omit-xml-declaration="no" indent="yes"/>

  <xsl:template match="CompleteMultipartUploadResult">
    <CompleteMultipartUploadResponse>
      <BucketName>
        <xsl:value-of select="Bucket"/>
      </BucketName>
      <Key>
        <xsl:value-of select="Key"/>
      </Key>
      <Location>
        <xsl:value-of select="Location"/>
      </Location>
      <ETag>
        <xsl:value-of select="ETag"/>
      </ETag>
  </CompleteMultipartUploadResponse>
  </xsl:template>
</xsl:stylesheet>
