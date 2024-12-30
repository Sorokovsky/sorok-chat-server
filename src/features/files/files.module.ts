import { Module } from "@nestjs/common";
import { FilesService } from "@features/files/files.service";
import { FilesController } from "@features/files/files.controller";

@Module({
  controllers: [FilesController],
  providers: [FilesService],
})
export class FilesModule {}
