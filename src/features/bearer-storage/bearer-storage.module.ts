import { Module } from "@nestjs/common";
import { BearerStorageService } from "./bearer-storage.service";

@Module({
  providers: [BearerStorageService],
  exports: [BearerStorageService],
})
export class BearerStorageModule {}
