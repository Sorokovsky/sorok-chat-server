import { Module } from "@nestjs/common";
import { BearerStorageService } from "@features/bearer-storage/bearer-storage.service";

@Module({
  providers: [BearerStorageService],
  exports: [BearerStorageService],
})
export class BearerStorageModule {}
