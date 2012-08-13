<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" exclude-result-prefixes="xsl">
  <xsl:output method="xml" omit-xml-declaration="no" indent="yes"/>

  <xsl:template match="InitiateMultipartUploadResult">
    <InitiateMultipartUploadResponse>
      <BucketName>
        <xsl:value-of select="Bucket"/>
      </BucketName>
      <Key>
        <xsl:value-of select="Key"/>
      </Key>
      <UploadId>
        <xsl:value-of select="UploadId"/>
      </UploadId>
    </InitiateMultipartUploadResponse>
  </xsl:template>
</xsl:stylesheet>
