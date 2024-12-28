import {
  Controller,
  Param,
  Post,
  UploadedFile,
  UseInterceptors,
} from "@nestjs/common";
import { FilesService } from "./files.service";
import { FileInterceptor } from "@nestjs/platform-express";
import { ApiBody, ApiConsumes } from "@nestjs/swagger";

@Controller("files")
export class FilesController {
  constructor(private readonly filesService: FilesService) {}

  @Post(":folder")
  @UseInterceptors(FileInterceptor("file"))
  @ApiConsumes("multipart/form-data")
  @ApiBody({
    schema: {
      type: "object",
      properties: {
        file: {
          type: "string",
          format: "binary",
        },
      },
    },
  })
  public uploadFile(
    @UploadedFile() file: Express.Multer.File,
    @Param("folder") folder: string,
  ) {
    return this.filesService.upload(file, folder);
  }
}
